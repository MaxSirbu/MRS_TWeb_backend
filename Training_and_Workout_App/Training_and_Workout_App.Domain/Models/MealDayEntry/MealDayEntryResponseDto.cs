using Training_and_Workout_App.Domain.Entities;

namespace Training_and_Workout_App.Domain.Models;

public class MealDayEntryResponseDto
{
    public int Id { get; set; }
    public string MealPlanIdentifier { get; set; } = string.Empty;
    public string DayId { get; set; } = string.Empty;
    public MealSlot MealSlot { get; set; }
    public List<FoodItemResponseDto> FoodItems { get; set; } = [];
}
