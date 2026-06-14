using Training_and_Workout_App.Domain.Models.MealPlan;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface IMealPlanAction
    : IMealPlanService
{
    Task<List<MealPlanSummaryDto>> GetSummariesByUserIdAsync(int userId);
    Task<MealCategoryFoodItemResponseDto?> UpdateItemQuantityAsync(
        int itemId,
        int currentUserId,
        bool isAdmin,
        MealItemQuantityUpdateDto dto);
}
