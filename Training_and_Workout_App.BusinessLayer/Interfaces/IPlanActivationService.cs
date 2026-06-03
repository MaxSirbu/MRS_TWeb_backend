using Training_and_Workout_App.Domain.Entities.PlanState;
using Training_and_Workout_App.Domain.Models.PlanActivation;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface IPlanActivationService
{
    // Returnează planul activ curent pentru un user și un tip de plan
    Task<PlanActivationResponseDto?> GetActiveAsync(int userId, PlanType planType);

    // Activează un plan nou (înlocuiește cel vechi dacă există)
    Task<PlanActivationResponseDto> ActivateAsync(int userId, PlanActivationCreateDto dto);

    // Dezactivează planul activ (îl șterge din DB)
    Task<bool> DeactivateAsync(int userId, PlanType planType);

    // Resetează ciclul (LastCycleResetAt = today)
    Task<PlanActivationResponseDto> ResetCycleAsync(int userId, PlanType planType);
}
