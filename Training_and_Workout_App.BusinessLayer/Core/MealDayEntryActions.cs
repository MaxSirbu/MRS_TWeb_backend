using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Entities.Exercise;
using Training_and_Workout_App.Domain.Entities.FAQ;
using Training_and_Workout_App.Domain.Entities.FoodItem;
using Training_and_Workout_App.Domain.Entities.MealDayEntry;
using Training_and_Workout_App.Domain.Entities.Plans;
using Training_and_Workout_App.Domain.Entities.PlanState;
using Training_and_Workout_App.Domain.Entities.Question;
using Training_and_Workout_App.Domain.Entities.QuestionnaireEntry;
using Training_and_Workout_App.Domain.Entities.User;
using Training_and_Workout_App.Domain.Entities.WorkoutHistory;
using Training_and_Workout_App.Domain.Entities.WorkoutTracking;
using Training_and_Workout_App.Domain.Models.DayPlan;
using Training_and_Workout_App.Domain.Models.Exercise;
using Training_and_Workout_App.Domain.Models.FAQ;
using Training_and_Workout_App.Domain.Models.FoodItem;
using Training_and_Workout_App.Domain.Models.MealDayEntry;
using Training_and_Workout_App.Domain.Models.MealPlan;
using Training_and_Workout_App.Domain.Models.PlanActivation;
using Training_and_Workout_App.Domain.Models.PlanCompletion;
using Training_and_Workout_App.Domain.Models.PlanCustomization;
using Training_and_Workout_App.Domain.Models.Question;
using Training_and_Workout_App.Domain.Models.QuestionnaireEntry;
using Training_and_Workout_App.Domain.Models.User;
using Training_and_Workout_App.Domain.Models.UserPlanFavorite;
using Training_and_Workout_App.Domain.Models.UserProfile;
using Training_and_Workout_App.Domain.Models.WorkoutPlan;
using Training_and_Workout_App.Domain.Models.WorkoutTracking;
using Microsoft.EntityFrameworkCore;
using Training_and_Workout_App.DataAccess.Context;

namespace Training_and_Workout_App.BusinessLayer.Core;

public class MealDayEntryActions(ApplicationDbContext context)
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
                context.MealDayEntryFoodItems.Add(new MealDayEntryFoodItemData
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
            var entry = new MealDayEntryData
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
                context.MealDayEntryFoodItems.Add(new MealDayEntryFoodItemData
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
