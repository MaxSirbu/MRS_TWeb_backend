using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Models;

public class WorkoutPlanCreateDto
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    public List<DayPlanCreateDto> Days { get; set; } = [];
}
