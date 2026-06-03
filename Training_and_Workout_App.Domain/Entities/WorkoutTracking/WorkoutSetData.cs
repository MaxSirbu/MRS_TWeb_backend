using Training_and_Workout_App.Domain.Entities.Plans;
using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities.WorkoutTracking;

public class WorkoutSetData
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Range(0.0, 1000.0)]
    public double Weight { get; set; }

    [Required]
    [Range(1, 500)]
    public int Reps { get; set; }

    [Range(0, int.MaxValue)]
    public int Order { get; set; }

    // FK -> WorkoutTrackingState
    public int? WorkoutTrackingStateId { get; set; }
    public WorkoutTrackingStateData? WorkoutTrackingState { get; set; }

    public int? DayPlanId { get; set; }
    public int? ExerciseId { get; set; }
    public DayPlanExerciseData? DayPlanExercise { get; set; }
}
