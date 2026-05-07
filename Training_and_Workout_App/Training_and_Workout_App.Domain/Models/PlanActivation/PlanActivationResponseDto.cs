using Training_and_Workout_App.Domain.Entities;

namespace Training_and_Workout_App.Domain.Models;

public class PlanActivationResponseDto
{
    public int Id { get; set; }
    public PlanType PlanType { get; set; }
    public string PlanIdentifier { get; set; } = string.Empty;
    public DateOnly ActivatedAt { get; set; }
    public int TotalDays { get; set; }
    public DateOnly? LastCycleResetAt { get; set; }
}
