using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface IMealDayEntryActions
{
    Task<List<MealDayEntryResponseDto>> GetByUserAndDayAsync(
        int userId,
        string mealPlanIdentifier,
        string dayId);

    Task<MealDayEntryResponseDto> UpsertAsync(int userId, MealDayEntryCreateDto dto);

    Task<bool> DeleteAsync(int id, int userId);
}
