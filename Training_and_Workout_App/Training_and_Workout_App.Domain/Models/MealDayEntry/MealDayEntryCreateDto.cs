using System.ComponentModel.DataAnnotations;
using Training_and_Workout_App.Domain.Entities;

namespace Training_and_Workout_App.Domain.Models;

public class MealDayEntryCreateDto
{
    [Required]
    [StringLength(100)]
    public string MealPlanIdentifier { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string DayId { get; set; } = string.Empty;

    [Required]
    public MealSlot MealSlot { get; set; }

    public List<int> FoodItemIds { get; set; } = [];
}
