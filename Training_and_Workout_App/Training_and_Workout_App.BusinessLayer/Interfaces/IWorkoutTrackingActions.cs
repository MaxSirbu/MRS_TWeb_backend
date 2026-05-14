using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface IWorkoutTrackingActions
{
    Task<WorkoutTrackingStateResponseDto?> GetByPlanIdAsync(int workoutPlanId);

    // Salvează seturile (înlocuiește cele vechi)
    Task<WorkoutTrackingStateResponseDto> SaveSetsAsync(
        int workoutPlanId,
        List<WorkoutSetDto> sets);

    // Salvează pauza
    Task<WorkoutTrackingStateResponseDto> SavePauseAsync(
        int workoutPlanId,
        PauseTimeDto pause);

    Task<bool> DeleteAsync(int workoutPlanId);
}
