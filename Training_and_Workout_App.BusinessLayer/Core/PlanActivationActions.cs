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

public class PlanActivationActions(ApplicationDbContext context)
{
    public async Task<PlanActivationResponseDto?> GetActiveAsync(int userId, PlanType planType)
    {
        var activation = await context.PlanActivations
            .FirstOrDefaultAsync(pa => pa.UserId == userId && pa.PlanType == planType);

        return activation is null ? null : MapToDto(activation);
    }

    public async Task<PlanActivationResponseDto> ActivateAsync(int userId, PlanActivationCreateDto dto)
    {
        // Dacă există deja o activare de același tip → o înlocuim (o ștergem mai întâi)
        var existing = await context.PlanActivations
            .FirstOrDefaultAsync(pa => pa.UserId == userId && pa.PlanType == dto.PlanType);

        if (existing is not null)
            context.PlanActivations.Remove(existing);

        var activation = new PlanActivationData
        {
            PlanType = dto.PlanType,
            PlanIdentifier = dto.PlanIdentifier,
            ActivatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
            TotalDays = dto.TotalDays,
            UserId = userId
        };

        context.PlanActivations.Add(activation);
        await context.SaveChangesAsync();

        return MapToDto(activation);
    }

    public async Task<bool> DeactivateAsync(int userId, PlanType planType)
    {
        var activation = await context.PlanActivations
            .FirstOrDefaultAsync(pa => pa.UserId == userId && pa.PlanType == planType);

        if (activation is null) return false;

        context.PlanActivations.Remove(activation);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<PlanActivationResponseDto> ResetCycleAsync(int userId, PlanType planType)
    {
        var activation = await context.PlanActivations
            .FirstOrDefaultAsync(pa => pa.UserId == userId && pa.PlanType == planType)
            ?? throw new KeyNotFoundException($"No active {planType} plan for user {userId}.");

        activation.LastCycleResetAt = DateOnly.FromDateTime(DateTime.UtcNow);
        await context.SaveChangesAsync();

        return MapToDto(activation);
    }

    private static PlanActivationResponseDto MapToDto(PlanActivationData pa) => new()
    {
        Id = pa.Id,
        PlanType = pa.PlanType,
        PlanIdentifier = pa.PlanIdentifier,
        ActivatedAt = pa.ActivatedAt,
        TotalDays = pa.TotalDays,
        LastCycleResetAt = pa.LastCycleResetAt
    };
}
