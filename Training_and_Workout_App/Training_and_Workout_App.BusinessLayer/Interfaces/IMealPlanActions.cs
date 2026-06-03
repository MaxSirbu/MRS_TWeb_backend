using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface IMealPlanActions
{
    Task<List<MealPlanResponseDto>> GetByUserIdAsync(int userId);
    Task<MealPlanResponseDto?> GetByIdAsync(int id);
    Task<MealPlanResponseDto> CreateAsync(int userId, MealPlanCreateDto dto);
    Task<MealPlanResponseDto> UpdateAsync(int id, MealPlanCreateDto dto);
    Task<bool> DeleteAsync(int id);
}
