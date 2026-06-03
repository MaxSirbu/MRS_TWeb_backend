using Training_and_Workout_App.Domain.Models.Exercise;
using Training_and_Workout_App.Domain.Models.WorkoutTracking;
namespace Training_and_Workout_App.Domain.Models.DayPlan;

public class WorkoutDayExerciseResponseDto
{
    public int DayPlanId { get; set; }
    public int ExerciseId { get; set; }
    public int Order { get; set; }
    public ExerciseResponseDto Exercise { get; set; } = new();
    public List<WorkoutSetDto> Sets { get; set; } = [];
}
