using Training_and_Workout_App.Domain.Entities.PlanState;
using Training_and_Workout_App.Domain.Models.PlanCompletion;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface IPlanCompletionAction
{
    Task<List<PlanCompletionResponseDto>> GetByUserAsync(int userId, PlanType? planType = null);
    Task<PlanCompletionResponseDto> MarkCompleteAsync(int userId, PlanCompletionCreateDto dto);
    Task<bool> UnmarkAsync(int userId, string dayToken, DateOnly dateKey);
}
