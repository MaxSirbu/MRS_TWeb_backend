namespace Training_and_Workout_App.Domain.Models;

public class CreateTimerSessionDto
{
    public int DurationSeconds { get; set; }
    public string? ExerciseName { get; set; }
    public string? Notes { get; set; }
}

public class UpdateTimerSessionDto
{
    public string? Status { get; set; }
    public string? Notes { get; set; }
}

public class TimerSessionResponseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int DurationSeconds { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ExerciseName { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class TimerSessionStatisticsDto
{
    public int TotalSessions { get; set; }
    public int TotalSeconds { get; set; }
    public int CompletedSessions { get; set; }
    public int AverageSessionSeconds { get; set; }
    public string? MostUsedExercise { get; set; }
}
