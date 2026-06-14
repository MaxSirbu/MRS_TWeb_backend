using Training_and_Workout_App.Domain.Entities.MealDayEntry;
using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Models.MealDayEntry;

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
