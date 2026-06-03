using Training_and_Workout_App.Domain.Models.PlanCustomization;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface IPlanCustomizationAction
{
    Task<List<PlanCustomizationDto>> GetByUserAsync(int userId);
    Task<PlanCustomizationDto> UpsertAsync(int userId, PlanCustomizationDto dto);
}
