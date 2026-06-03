using Training_and_Workout_App.Domain.Entities.PlanState;
using Training_and_Workout_App.Domain.Models.PlanActivation;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface IPlanActivationAction
{
    Task<PlanActivationResponseDto?> GetActiveAsync(int userId, PlanType planType);
    Task<PlanActivationResponseDto> ActivateAsync(int userId, PlanActivationCreateDto dto);
    Task<bool> DeactivateAsync(int userId, PlanType planType);
    Task<PlanActivationResponseDto> ResetCycleAsync(int userId, PlanType planType);
}
