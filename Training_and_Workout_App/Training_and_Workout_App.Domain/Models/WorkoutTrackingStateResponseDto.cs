namespace Training_and_Workout_App.Domain.Models;

public class WorkoutTrackingStateResponseDto
{
    public Guid Id { get; set; }
    public PauseTimeDto PauseTime { get; set; } = new PauseTimeDto();
    public List<WorkoutSetDto> Sets { get; set; } = [];
}
