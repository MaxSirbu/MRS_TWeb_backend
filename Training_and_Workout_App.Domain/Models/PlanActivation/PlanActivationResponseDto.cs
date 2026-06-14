using Training_and_Workout_App.Domain.Entities.PlanState;

namespace Training_and_Workout_App.Domain.Models.PlanActivation;

public class PlanActivationResponseDto
{
    public int Id { get; set; }
    public PlanType PlanType { get; set; }
    public string PlanIdentifier { get; set; } = string.Empty;
    public DateOnly ActivatedAt { get; set; }
    public int TotalDays { get; set; }
    public DateOnly? LastCycleResetAt { get; set; }
}
