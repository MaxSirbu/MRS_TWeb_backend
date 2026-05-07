using Training_and_Workout_App.Domain.Entities;

namespace Training_and_Workout_App.Domain.Models;

public class PlanCompletionResponseDto
{
    public int Id { get; set; }
    public PlanType PlanType { get; set; }
    public string DayToken { get; set; } = string.Empty;
    public DateOnly DateKey { get; set; }
}
