using Training_and_Workout_App.Domain.Entities.Plans;
using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities.WorkoutTracking;

public class WorkoutTrackingStateData
{
    [Key]
    public int Id { get; set; }

    // one-to-one with PauseTime
    public PauseTimeData PauseTime { get; set; } = null!;

    // one-to-many with WorkoutSet
    public ICollection<WorkoutSetData> Sets { get; set; } = [];

    // FK -> WorkoutPlan (one-to-one)
    public int WorkoutPlanId { get; set; }
    public WorkoutPlanData WorkoutPlan { get; set; } = null!;
}
