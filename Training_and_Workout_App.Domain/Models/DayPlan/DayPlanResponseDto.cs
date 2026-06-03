using Training_and_Workout_App.Domain.Models.Exercise;
namespace Training_and_Workout_App.Domain.Models.DayPlan;

public class DayPlanResponseDto
{
    public int Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public int DayNumber { get; set; }
    public List<ExerciseResponseDto> Exercises { get; set; } = [];
    public List<WorkoutDayExerciseResponseDto> DayExercises { get; set; } = [];
}
