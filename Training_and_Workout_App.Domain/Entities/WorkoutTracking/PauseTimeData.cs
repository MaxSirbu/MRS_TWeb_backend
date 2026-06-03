using Training_and_Workout_App.Domain.Entities.Plans;
using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities.WorkoutTracking;

public class PauseTimeData
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Range(0, 60)]
    public int Minutes { get; set; }

    [Required]
    [Range(0, 59)]
    public int Seconds { get; set; }

    // Legacy plan-level pause. New workout plans store pause per DayPlanExercise.
    public int? WorkoutTrackingStateId { get; set; }
    public WorkoutTrackingStateData? WorkoutTrackingState { get; set; }

    public int? DayPlanId { get; set; }
    public int? ExerciseId { get; set; }
    public DayPlanExerciseData? DayPlanExercise { get; set; }
}
