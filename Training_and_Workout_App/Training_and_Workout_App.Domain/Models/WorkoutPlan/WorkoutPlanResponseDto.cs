namespace Training_and_Workout_App.Domain.Models;

public class WorkoutPlanResponseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }   // necesar pentru ownership check
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<DayPlanResponseDto> Days { get; set; } = [];
    public WorkoutTrackingStateResponseDto? WorkoutTracking { get; set; }
}
