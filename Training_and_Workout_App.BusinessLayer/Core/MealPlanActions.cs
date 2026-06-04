using Training_and_Workout_App.Domain.Entities.FoodItem;
using Training_and_Workout_App.Domain.Entities.Plans;
using Training_and_Workout_App.Domain.Models.FoodItem;
using Training_and_Workout_App.Domain.Models.MealPlan;
using Microsoft.EntityFrameworkCore;
using Training_and_Workout_App.DataAccess.Context;

namespace Training_and_Workout_App.BusinessLayer.Core;

public class MealPlanActions(ApplicationDbContext context)
{
    public async Task<List<MealPlanSummaryDto>> GetSummariesByUserIdAsync(int userId)
    {
        var plans = await context.MealPlans
            .AsNoTracking()
            .Where(mp => mp.UserId == userId)
            .OrderByDescending(mp => mp.UpdatedAt)
            .Select(mp => new MealPlanSummaryDto
            {
                Id = mp.Id,
                UserId = mp.UserId,
                Name = mp.Name,
                UpdatedAt = mp.UpdatedAt,
                Meals = mp.Meals,
            })
            .ToListAsync();

        var planIds = plans.Select(plan => plan.Id).ToList();
        var dayStats = await context.MealPlanDays
            .AsNoTracking()
            .Where(day => planIds.Contains(day.MealPlanId))
            .Select(day => new
            {
                day.MealPlanId,
                Kcal = day.Categories
                    .SelectMany(category => category.Items)
                    .Sum(item => item.Kcal)
            })
            .ToListAsync();

        var statsByPlan = dayStats
            .GroupBy(day => day.MealPlanId)
            .ToDictionary(
                group => group.Key,
                group => new
                {
                    DayCount = group.Count(),
                    KcalPerDay = group.Count() == 0 ? 0 : (int)Math.Round(group.Sum(day => day.Kcal) / group.Count())
                });

        foreach (var plan in plans)
        {
            if (!statsByPlan.TryGetValue(plan.Id, out var stats)) continue;
            plan.DayCount = stats.DayCount;
            plan.KcalPerDay = stats.KcalPerDay;
        }

        return plans;
    }

    public async Task<List<MealPlanResponseDto>> GetByUserIdAsync(int userId)
    {
        var plans = await context.MealPlans
            .Where(mp => mp.UserId == userId)
            .ToListAsync();

        return plans.Select(MapToDto).ToList();
    }

    public async Task<MealPlanResponseDto?> GetByIdAsync(int id)
    {
        var mp = await context.MealPlans
            .FirstOrDefaultAsync(p => p.Id == id);
        if (mp is null) return null;

        return MapToDto(mp);
    }

    public async Task<MealPlanResponseDto> CreateAsync(int userId, MealPlanCreateDto dto)
    {
        var plan = new MealPlanData
        {
            Name = dto.Name,
            Meals = dto.Meals,
            UpdatedAt = DateTime.UtcNow,
            UserId = userId
        };

        context.MealPlans.Add(plan);
        await context.SaveChangesAsync();
        await ReplaceDaysAsync(plan.Id, dto.Days);

        return (await GetByIdAsync(plan.Id))!;
    }

    public async Task<MealPlanResponseDto> UpdateAsync(int id, MealPlanCreateDto dto)
    {
        var plan = await context.MealPlans.FindAsync(id)
            ?? throw new KeyNotFoundException($"MealPlan {id} not found.");

        plan.Name = dto.Name;
        plan.Meals = dto.Meals;
        plan.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        await ReplaceDaysAsync(plan.Id, dto.Days);

        return (await GetByIdAsync(plan.Id))!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var plan = await context.MealPlans.FindAsync(id);
        if (plan is null) return false;

        context.MealPlans.Remove(plan);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<MealCategoryFoodItemResponseDto?> UpdateItemQuantityAsync(
        int itemId,
        int currentUserId,
        bool isAdmin,
        MealItemQuantityUpdateDto dto)
    {
        var item = await context.MealCategoryFoodItems
            .Include(i => i.FoodItem)
            .Include(i => i.MealCategory)
                .ThenInclude(category => category.MealPlanDay)
                    .ThenInclude(day => day.MealPlan)
            .FirstOrDefaultAsync(i => i.Id == itemId);

        if (item is null) return null;
        if (!isAdmin && item.MealCategory.MealPlanDay.MealPlan.UserId != currentUserId) return null;

        var multiplier = dto.QuantityGrams / Math.Max(item.FoodItem.Grams, 1);
        item.QuantityGrams = dto.QuantityGrams;
        item.Kcal = Math.Round(item.FoodItem.Kcal * multiplier, 2);
        item.Protein = Math.Round(item.FoodItem.Protein * multiplier, 2);
        item.Carbs = Math.Round(item.FoodItem.Carbs * multiplier, 2);
        item.Fats = Math.Round(item.FoodItem.Fats * multiplier, 2);

        await context.SaveChangesAsync();

        return new MealCategoryFoodItemResponseDto
        {
            Id = item.Id,
            FoodItemId = item.FoodItemId,
            Order = item.Order,
            QuantityGrams = item.QuantityGrams,
            Kcal = item.Kcal,
            Protein = item.Protein,
            Carbs = item.Carbs,
            Fats = item.Fats,
            FoodItem = MapFoodItem(item.FoodItem)
        };
    }

    private async Task ReplaceDaysAsync(int mealPlanId, List<MealPlanDayCreateDto> days)
    {
        var existingDays = await context.MealPlanDays
            .Where(day => day.MealPlanId == mealPlanId)
            .ToListAsync();

        context.MealPlanDays.RemoveRange(existingDays);
        await context.SaveChangesAsync();

        if (days.Count == 0) return;

        var foodIds = days
            .SelectMany(day => day.Categories)
            .SelectMany(category => category.Items)
            .Select(item => item.FoodItemId)
            .Distinct()
            .ToList();

        var foodItems = await context.FoodItems
            .Where(item => foodIds.Contains(item.Id))
            .ToDictionaryAsync(item => item.Id);

        foreach (var dayDto in days.OrderBy(day => day.DayNumber))
        {
            var day = new MealPlanDayData
            {
                MealPlanId = mealPlanId,
                Label = dayDto.Label,
                DayNumber = dayDto.DayNumber,
            };

            foreach (var categoryDto in dayDto.Categories.OrderBy(category => category.Order))
            {
                var category = new MealCategoryData
                {
                    Slot = categoryDto.Slot,
                    Order = categoryDto.Order,
                };

                foreach (var itemDto in categoryDto.Items.OrderBy(item => item.Order))
                {
                    if (!foodItems.TryGetValue(itemDto.FoodItemId, out var foodItem))
                    {
                        continue;
                    }

                    var multiplier = itemDto.QuantityGrams / Math.Max(foodItem.Grams, 1);
                    category.Items.Add(new MealCategoryFoodItemData
                    {
                        FoodItemId = itemDto.FoodItemId,
                        Order = itemDto.Order,
                        QuantityGrams = itemDto.QuantityGrams,
                        Kcal = Math.Round(foodItem.Kcal * multiplier, 2),
                        Protein = Math.Round(foodItem.Protein * multiplier, 2),
                        Carbs = Math.Round(foodItem.Carbs * multiplier, 2),
                        Fats = Math.Round(foodItem.Fats * multiplier, 2),
                    });
                }

                day.Categories.Add(category);
            }

            context.MealPlanDays.Add(day);
        }

        await context.SaveChangesAsync();
    }

    private MealPlanResponseDto MapToDto(MealPlanData plan)
    {
        var days = context.MealPlanDays
            .Where(day => day.MealPlanId == plan.Id)
            .Include(day => day.Categories)
                .ThenInclude(category => category.Items)
                    .ThenInclude(item => item.FoodItem)
            .AsSplitQuery()
            .ToList();

        return new MealPlanResponseDto
        {
            Id = plan.Id,
            UserId = plan.UserId,
            Name = plan.Name,
            UpdatedAt = plan.UpdatedAt,
            Meals = plan.Meals,
            Days = days
                .OrderBy(day => day.DayNumber)
                .Select(day => new MealPlanDayResponseDto
                {
                    Id = day.Id,
                    Label = day.Label,
                    DayNumber = day.DayNumber,
                    Categories = day.Categories
                        .OrderBy(category => category.Order)
                        .Select(category => new MealCategoryResponseDto
                        {
                            Id = category.Id,
                            Slot = category.Slot,
                            Order = category.Order,
                            Items = category.Items
                                .OrderBy(item => item.Order)
                                .Select(item => new MealCategoryFoodItemResponseDto
                                {
                                    Id = item.Id,
                                    FoodItemId = item.FoodItemId,
                                    Order = item.Order,
                                    QuantityGrams = item.QuantityGrams,
                                    Kcal = item.Kcal,
                                    Protein = item.Protein,
                                    Carbs = item.Carbs,
                                    Fats = item.Fats,
                                    FoodItem = MapFoodItem(item.FoodItem)
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToList()
        };
    }

    private static FoodItemResponseDto MapFoodItem(FoodItemData foodItem) => new()
    {
        Id = foodItem.Id,
        Name = foodItem.Name,
        Kcal = foodItem.Kcal,
        Protein = foodItem.Protein,
        Carbs = foodItem.Carbs,
        Fats = foodItem.Fats,
        Grams = foodItem.Grams,
        ImageUrl = foodItem.ImageUrl,
        Category = foodItem.Category,
        Description = foodItem.Description,
        ItemType = foodItem.ItemType,
        Recommended = foodItem.Recommended
    };
}
