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

public class UserPlanFavoriteActions(ApplicationDbContext context)
{
    public async Task<List<UserPlanFavoriteDto>> GetByUserAsync(int userId)
    {
        return await context.UserPlanFavorites
            .Where(f => f.UserId == userId)
            .Select(f => new UserPlanFavoriteDto
            {
                PlanType = f.PlanType,
                PlanIdentifier = f.PlanIdentifier
            })
            .ToListAsync();
    }

    public async Task<UserPlanFavoriteDto> AddAsync(int userId, UserPlanFavoriteDto dto)
    {
        // Verifică dacă planul este deja la favorite (evită duplicate)
        var exists = await context.UserPlanFavorites.AnyAsync(f =>
            f.UserId == userId &&
            f.PlanType == dto.PlanType &&
            f.PlanIdentifier == dto.PlanIdentifier);

        if (!exists)
        {
            context.UserPlanFavorites.Add(new UserPlanFavoriteData
            {
                PlanType = dto.PlanType,
                PlanIdentifier = dto.PlanIdentifier,
                UserId = userId
            });

            await context.SaveChangesAsync();
        }

        return dto;
    }

    public async Task<bool> RemoveAsync(int userId, PlanType planType, string planIdentifier)
    {
        var favorite = await context.UserPlanFavorites
            .FirstOrDefaultAsync(f =>
                f.UserId == userId &&
                f.PlanType == planType &&
                f.PlanIdentifier == planIdentifier);

        if (favorite is null) return false;

        context.UserPlanFavorites.Remove(favorite);
        await context.SaveChangesAsync();
        return true;
    }
}
