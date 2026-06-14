using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities;

public class TimerSession
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    // FK -> User
    public User User { get; set; } = null!;

    [Required]
    [Range(1, 86400)] // 1 second to 24 hours
    public int DurationSeconds { get; set; }

    [Required]
    public DateTime StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    [Required]
    public TimerStatus Status { get; set; } = TimerStatus.Completed;

    [StringLength(200)]
    public string? ExerciseName { get; set; }

    [StringLength(1000)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum TimerStatus
{
    Running = 0,
    Paused = 1,
    Completed = 2,
    Abandoned = 3
}
