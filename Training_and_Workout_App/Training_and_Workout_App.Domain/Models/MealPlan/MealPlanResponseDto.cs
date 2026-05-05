namespace Training_and_Workout_App.Domain.Models;

public class MealPlanResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
    public int Meals { get; set; }
}
