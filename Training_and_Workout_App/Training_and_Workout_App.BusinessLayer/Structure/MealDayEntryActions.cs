using Microsoft.EntityFrameworkCore;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.DataAccess.Context;
using Training_and_Workout_App.Domain.Entities;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.BusinessLayer.Structure;

public class MealDayEntryActions(ApplicationDbContext context) : IMealDayEntryActions
{
    public async Task<List<MealDayEntryResponseDto>> GetByUserAndDayAsync(
        int userId, string mealPlanIdentifier, string dayId)
    {
        return await context.MealDayEntries
            .Where(m => m.UserId == userId
                     && m.MealPlanIdentifier == mealPlanIdentifier
                     && m.DayId == dayId)
            .Include(m => m.MealDayEntryFoodItems)
                .ThenInclude(mf => mf.FoodItem)
            .Select(m => new MealDayEntryResponseDto
            {
                Id = m.Id,
                MealPlanIdentifier = m.MealPlanIdentifier,
                DayId = m.DayId,
                MealSlot = m.MealSlot,
                FoodItems = m.MealDayEntryFoodItems
                    .OrderBy(mf => mf.Order)
                    .Select(mf => new FoodItemResponseDto
                    {
                        Id = mf.FoodItem.Id,
                        Name = mf.FoodItem.Name,
                        Kcal = mf.FoodItem.Kcal,
                        Protein = mf.FoodItem.Protein,
                        Carbs = mf.FoodItem.Carbs,
                        Fats = mf.FoodItem.Fats,
                        Grams = mf.FoodItem.Grams,
                        ImageUrl = mf.FoodItem.ImageUrl,
                        Category = mf.FoodItem.Category,
                        Description = mf.FoodItem.Description,
                        ItemType = mf.FoodItem.ItemType,
                        Recommended = mf.FoodItem.Recommended
                    }).ToList()
            })
            .ToListAsync();
    }

    public async Task<MealDayEntryResponseDto> UpsertAsync(int userId, MealDayEntryCreateDto dto)
    {
        // Caută entry existent pentru aceeași combinație user+plan+zi+slot
        var existing = await context.MealDayEntries
            .Include(m => m.MealDayEntryFoodItems)
            .FirstOrDefaultAsync(m =>
                m.UserId == userId &&
                m.MealPlanIdentifier == dto.MealPlanIdentifier &&
                m.DayId == dto.DayId &&
                m.MealSlot == dto.MealSlot);

        if (existing is not null)
        {
            // Actualizează: șterge food items vechi, adaugă noile
            context.MealDayEntryFoodItems.RemoveRange(existing.MealDayEntryFoodItems);

            int order = 0;
            foreach (var foodId in dto.FoodItemIds)
            {
                context.MealDayEntryFoodItems.Add(new MealDayEntryFoodItem
                {
                    MealDayEntryId = existing.Id,
                    FoodItemId = foodId,
                    Order = order++
                });
            }

            await context.SaveChangesAsync();
            return await GetEntryDtoAsync(existing.Id);
        }
        else
        {
            // Creează entry nou
            var entry = new MealDayEntry
            {
                MealPlanIdentifier = dto.MealPlanIdentifier,
                DayId = dto.DayId,
                MealSlot = dto.MealSlot,
                UserId = userId
            };

            context.MealDayEntries.Add(entry);
            await context.SaveChangesAsync();

            int order = 0;
            foreach (var foodId in dto.FoodItemIds)
            {
                context.MealDayEntryFoodItems.Add(new MealDayEntryFoodItem
                {
                    MealDayEntryId = entry.Id,
                    FoodItemId = foodId,
                    Order = order++
                });
            }

            await context.SaveChangesAsync();
            return await GetEntryDtoAsync(entry.Id);
        }
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        // Verifică că entry-ul aparține userului (securitate)
        var entry = await context.MealDayEntries
            .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

        if (entry is null) return false;

        context.MealDayEntries.Remove(entry);
        await context.SaveChangesAsync();
        return true;
    }

    // Helper: reîncarcă entry-ul cu include pentru a returna datele complete
    private async Task<MealDayEntryResponseDto> GetEntryDtoAsync(int entryId)
    {
        var m = await context.MealDayEntries
            .Include(m => m.MealDayEntryFoodItems)
                .ThenInclude(mf => mf.FoodItem)
            .FirstAsync(m => m.Id == entryId);

        return new MealDayEntryResponseDto
        {
            Id = m.Id,
            MealPlanIdentifier = m.MealPlanIdentifier,
            DayId = m.DayId,
            MealSlot = m.MealSlot,
            FoodItems = m.MealDayEntryFoodItems
                .OrderBy(mf => mf.Order)
                .Select(mf => new FoodItemResponseDto
                {
                    Id = mf.FoodItem.Id,
                    Name = mf.FoodItem.Name,
                    Kcal = mf.FoodItem.Kcal,
                    Protein = mf.FoodItem.Protein,
                    Carbs = mf.FoodItem.Carbs,
                    Fats = mf.FoodItem.Fats,
                    Grams = mf.FoodItem.Grams,
                    ImageUrl = mf.FoodItem.ImageUrl,
                    Category = mf.FoodItem.Category,
                    Description = mf.FoodItem.Description,
                    ItemType = mf.FoodItem.ItemType,
                    Recommended = mf.FoodItem.Recommended
                }).ToList()
        };
    }
}
