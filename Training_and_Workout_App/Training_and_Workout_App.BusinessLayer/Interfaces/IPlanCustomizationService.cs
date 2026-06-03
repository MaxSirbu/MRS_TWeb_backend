using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface IPlanCustomizationService
{
    // Toate customizările unui user
    Task<List<PlanCustomizationDto>> GetByUserAsync(int userId);

    // Dacă există pentru (userId, planType, planIdentifier) → update; altfel → insert
    Task<PlanCustomizationDto> UpsertAsync(int userId, PlanCustomizationDto dto);
}
