using Training_and_Workout_App.Domain.Models.MealDayEntry;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface IMealDayEntryService
{
    // Returnează toate mesele unui user pentru un plan+zi specific
    Task<List<MealDayEntryResponseDto>> GetByUserAndDayAsync(
        int userId,
        string mealPlanIdentifier,
        string dayId);

    // Dacă entry-ul există → actualizează food items; dacă nu → creează
    Task<MealDayEntryResponseDto> UpsertAsync(int userId, MealDayEntryCreateDto dto);

    Task<bool> DeleteAsync(int id, int userId);
}
