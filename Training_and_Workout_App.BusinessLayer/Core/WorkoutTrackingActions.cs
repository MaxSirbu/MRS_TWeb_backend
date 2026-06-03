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

public class WorkoutTrackingActions(ApplicationDbContext context)
{
    public async Task<WorkoutTrackingStateResponseDto?> GetByPlanIdAsync(int workoutPlanId)
    {
        var state = await context.WorkoutTrackingStates
            .Include(wt => wt.Sets)
            .Include(wt => wt.PauseTime)
            .FirstOrDefaultAsync(wt => wt.WorkoutPlanId == workoutPlanId);

        return state is null ? null : MapToDto(state);
    }

    public async Task<WorkoutTrackingStateResponseDto> SaveSetsAsync(
        int workoutPlanId, List<WorkoutSetDto> sets)
    {
        var state = await GetOrCreateStateAsync(workoutPlanId);

        // Șterge seturile vechi și adaugă noile
        var oldSets = context.WorkoutSets.Where(s => s.WorkoutTrackingStateId == state.Id);
        context.WorkoutSets.RemoveRange(oldSets);

        foreach (var setDto in sets)
        {
            context.WorkoutSets.Add(new WorkoutSetData
            {
                Weight = setDto.Weight,
                Reps = setDto.Reps,
                WorkoutTrackingStateId = state.Id
            });
        }

        await context.SaveChangesAsync();

        // Reload cu include
        await context.Entry(state).Collection(s => s.Sets).LoadAsync();
        return MapToDto(state);
    }

    public async Task<WorkoutTrackingStateResponseDto> SavePauseAsync(
        int workoutPlanId, PauseTimeDto pause)
    {
        var state = await GetOrCreateStateAsync(workoutPlanId);

        var existingPause = await context.PauseTimes
            .FirstOrDefaultAsync(pt => pt.WorkoutTrackingStateId == state.Id);

        if (existingPause is not null)
        {
            // Actualizează pauza existentă
            existingPause.Minutes = pause.Minutes;
            existingPause.Seconds = pause.Seconds;
        }
        else
        {
            // Creează pauza nouă
            context.PauseTimes.Add(new PauseTimeData
            {
                Minutes = pause.Minutes,
                Seconds = pause.Seconds,
                WorkoutTrackingStateId = state.Id
            });
        }

        await context.SaveChangesAsync();

        // Reload cu include
        await context.Entry(state).Reference(s => s.PauseTime).LoadAsync();
        await context.Entry(state).Collection(s => s.Sets).LoadAsync();
        return MapToDto(state);
    }

    public async Task<bool> DeleteAsync(int workoutPlanId)
    {
        var state = await context.WorkoutTrackingStates
            .FirstOrDefaultAsync(wt => wt.WorkoutPlanId == workoutPlanId);

        if (state is null) return false;

        context.WorkoutTrackingStates.Remove(state);
        await context.SaveChangesAsync();
        return true;
    }

    // Găsește sau creează un WorkoutTrackingState pentru un plan dat
    private async Task<WorkoutTrackingStateData> GetOrCreateStateAsync(int workoutPlanId)
    {
        var state = await context.WorkoutTrackingStates
            .FirstOrDefaultAsync(wt => wt.WorkoutPlanId == workoutPlanId);

        if (state is not null) return state;

        state = new WorkoutTrackingStateData { WorkoutPlanId = workoutPlanId };
        context.WorkoutTrackingStates.Add(state);
        await context.SaveChangesAsync();

        // Creează și PauseTime default asociat
        context.PauseTimes.Add(new PauseTimeData
        {
            Minutes = 1,
            Seconds = 30,
            WorkoutTrackingStateId = state.Id
        });
        await context.SaveChangesAsync();

        return state;
    }

    private static WorkoutTrackingStateResponseDto MapToDto(WorkoutTrackingStateData wt) => new()
    {
        Id = wt.Id,
        Sets = wt.Sets?
            .Select(s => new WorkoutSetDto { Weight = s.Weight, Reps = s.Reps })
            .ToList() ?? [],
        PauseTime = wt.PauseTime is null
            ? new PauseTimeDto { Minutes = 0, Seconds = 0 }
            : new PauseTimeDto { Minutes = wt.PauseTime.Minutes, Seconds = wt.PauseTime.Seconds }
    };
}
