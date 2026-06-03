using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities.WorkoutHistory;

public class WorkoutCompletionSetData
{
    [Key]
    public int Id { get; set; }

    public int WorkoutCompletionExerciseId { get; set; }
    public WorkoutCompletionExerciseData WorkoutCompletionExercise { get; set; } = null!;

    [Range(1, 500)]
    public int SetNumber { get; set; }

    [Range(0.0, 1000.0)]
    public double Weight { get; set; }

    [Range(0, 500)]
    public int Reps { get; set; }

    [Range(0.0, double.MaxValue)]
    public double Volume { get; set; }
}
