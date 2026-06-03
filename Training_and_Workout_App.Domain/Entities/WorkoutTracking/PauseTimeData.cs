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

    // FK -> WorkoutTrackingState (one-to-one)
    public int WorkoutTrackingStateId { get; set; }
    public WorkoutTrackingStateData WorkoutTrackingState { get; set; } = null!;
}
