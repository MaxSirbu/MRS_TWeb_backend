using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Models;

public class MealPlanCreateDto
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [Range(1, 20)]
    public int Meals { get; set; }
}
