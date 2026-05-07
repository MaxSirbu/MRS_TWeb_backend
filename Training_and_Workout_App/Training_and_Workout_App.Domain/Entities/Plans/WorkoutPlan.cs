using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities;

public class WorkoutPlan
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    // FK -> User
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    // one-to-many with DayPlan
    public ICollection<DayPlan> Days { get; set; } = [];

    // one-to-one with WorkoutTrackingState (nullable: planul poate exista fara tracking)
    public WorkoutTrackingState? WorkoutTracking { get; set; }
}
