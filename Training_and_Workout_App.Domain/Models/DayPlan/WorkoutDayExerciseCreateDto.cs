using Training_and_Workout_App.Domain.Models.WorkoutTracking;
using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Models.DayPlan;

public class WorkoutDayExerciseCreateDto
{
    [Required]
    public int ExerciseId { get; set; }

    [Range(0, int.MaxValue)]
    public int Order { get; set; }

    public List<WorkoutSetDto> Sets { get; set; } = [];
}
