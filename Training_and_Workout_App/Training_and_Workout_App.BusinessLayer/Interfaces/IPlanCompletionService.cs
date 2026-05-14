using Training_and_Workout_App.Domain.Entities;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface IPlanCompletionService
{
    Task<List<PlanCompletionResponseDto>> GetByUserAsync(int userId, PlanType? planType = null);
    Task<PlanCompletionResponseDto> MarkCompleteAsync(int userId, PlanCompletionCreateDto dto);
    Task<bool> UnmarkAsync(int userId, string dayToken, DateOnly dateKey);
}
