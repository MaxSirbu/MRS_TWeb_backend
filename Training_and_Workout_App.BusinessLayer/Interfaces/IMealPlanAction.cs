using Training_and_Workout_App.Domain.Models.MealPlan;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface IMealPlanAction
{
    Task<List<MealPlanSummaryDto>> GetSummariesByUserIdAsync(int userId);
    Task<List<MealPlanResponseDto>> GetByUserIdAsync(int userId);
    Task<MealPlanResponseDto?> GetByIdAsync(int id);
    Task<MealPlanResponseDto> CreateAsync(int userId, MealPlanCreateDto dto);
    Task<MealPlanResponseDto> UpdateAsync(int id, MealPlanCreateDto dto);
    Task<MealCategoryFoodItemResponseDto?> UpdateItemQuantityAsync(
        int itemId,
        int currentUserId,
        bool isAdmin,
        MealItemQuantityUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
