using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities;

public class WorkoutSet
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Range(0.0, 1000.0)]
    public double Weight { get; set; }

    [Required]
    [Range(1, 500)]
    public int Reps { get; set; }

    // FK -> WorkoutTrackingState
    public int WorkoutTrackingStateId { get; set; }
    public WorkoutTrackingState WorkoutTrackingState { get; set; } = null!;
}
