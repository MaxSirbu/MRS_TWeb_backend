using Training_and_Workout_App.Domain.Entities.PlanState;
using Training_and_Workout_App.Domain.Models.PlanCompletion;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface IPlanCompletionService
{
    // Toate zilele completate ale unui user (opțional filtrat după tip plan)
    Task<List<PlanCompletionResponseDto>> GetByUserAsync(int userId, PlanType? planType = null);

    // Marchează o zi ca finalizată
    Task<PlanCompletionResponseDto> MarkCompleteAsync(int userId, PlanCompletionCreateDto dto);

    // Demarchează (șterge) o zi finalizată
    Task<bool> UnmarkAsync(int userId, string dayToken, DateOnly dateKey);
}
