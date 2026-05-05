namespace Training_and_Workout_App.Domain.Models;

public class DayPlanResponseDto
{
    public int Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public List<ExerciseResponseDto> Exercises { get; set; } = [];
}
