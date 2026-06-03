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

public class PlanCustomizationActions(ApplicationDbContext context)
{
    public async Task<List<PlanCustomizationDto>> GetByUserAsync(int userId)
    {
        return await context.PlanCustomizations
            .Where(pc => pc.UserId == userId)
            .Select(pc => new PlanCustomizationDto
            {
                PlanType = pc.PlanType,
                PlanIdentifier = pc.PlanIdentifier,
                ColorId = pc.ColorId,
                ImageUrl = pc.ImageUrl
            })
            .ToListAsync();
    }

    public async Task<PlanCustomizationDto> UpsertAsync(int userId, PlanCustomizationDto dto)
    {
        var existing = await context.PlanCustomizations
            .FirstOrDefaultAsync(pc =>
                pc.UserId == userId &&
                pc.PlanType == dto.PlanType &&
                pc.PlanIdentifier == dto.PlanIdentifier);

        if (existing is not null)
        {
            // Actualizează câmpurile existente
            existing.ColorId = dto.ColorId;
            existing.ImageUrl = dto.ImageUrl;
        }
        else
        {
            // Creează customizare nouă
            context.PlanCustomizations.Add(new PlanCustomizationData
            {
                PlanType = dto.PlanType,
                PlanIdentifier = dto.PlanIdentifier,
                ColorId = dto.ColorId,
                ImageUrl = dto.ImageUrl,
                UserId = userId
            });
        }

        await context.SaveChangesAsync();
        return dto;
    }
}
