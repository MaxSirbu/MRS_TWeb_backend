using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities;

public class WorkoutPlan
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    // FK -> User
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    // one-to-many with DayPlan
    public ICollection<DayPlan> Days { get; set; } = [];

    // one-to-one with WorkoutTrackingState
    public WorkoutTrackingState WorkoutTracking { get; set; } = null!;
}
