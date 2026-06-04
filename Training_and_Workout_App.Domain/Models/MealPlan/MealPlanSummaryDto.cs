namespace Training_and_Workout_App.Domain.Models.MealPlan;

public class MealPlanSummaryDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
    public int Meals { get; set; }
    public int DayCount { get; set; }
    public int KcalPerDay { get; set; }
}
