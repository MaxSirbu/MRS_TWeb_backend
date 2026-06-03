using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Models.DayPlan;

public class DayPlanCreateDto
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Label { get; set; } = string.Empty;

    public List<int> ExerciseIds { get; set; } = [];

    public List<WorkoutDayExerciseCreateDto> Exercises { get; set; } = [];
}
