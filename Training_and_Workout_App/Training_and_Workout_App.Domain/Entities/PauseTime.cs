using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities;

public class PauseTime
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Range(0, 60)]
    public int Minutes { get; set; }

    [Required]
    [Range(0, 59)]
    public int Seconds { get; set; }

    // FK -> WorkoutTrackingState (one-to-one)
    public int WorkoutTrackingStateId { get; set; }
    public WorkoutTrackingState WorkoutTrackingState { get; set; } = null!;
}
