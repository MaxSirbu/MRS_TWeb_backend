using Training_and_Workout_App.Domain.Entities;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface IWorkoutPlanActions
{
    Task<List<WorkoutPlanResponseDto>> GetByUserIdAsync(int userId);
    Task<WorkoutPlanResponseDto?> GetByIdAsync(int id);
    Task<WorkoutPlanResponseDto> CreateAsync(int userId, WorkoutPlanCreateDto dto);
    Task<WorkoutPlanResponseDto> UpdateAsync(int id, WorkoutPlanCreateDto dto);
    Task<bool> DeleteAsync(int id);
}