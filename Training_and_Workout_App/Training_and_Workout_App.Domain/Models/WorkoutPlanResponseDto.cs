namespace Training_and_Workout_App.Domain.Models;

public class WorkoutPlanResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<DayPlanResponseDto> Days { get; set; } = [];
    public WorkoutTrackingStateResponseDto? WorkoutTracking { get; set; }
}
