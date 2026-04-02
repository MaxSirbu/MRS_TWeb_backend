using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities;

public class WorkoutTrackingState
{
    [Key]
    public Guid Id { get; set; }

    // one-to-one with PauseTime
    public PauseTime PauseTime { get; set; } = null!;

    // one-to-many with WorkoutSet
    public ICollection<WorkoutSet> Sets { get; set; } = [];

    // FK -> WorkoutPlan (one-to-one)
    public Guid WorkoutPlanId { get; set; }
    public WorkoutPlan WorkoutPlan { get; set; } = null!;
}
