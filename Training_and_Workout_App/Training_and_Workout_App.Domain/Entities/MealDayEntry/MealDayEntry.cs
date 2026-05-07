using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities;

public class MealDayEntry
{
    [Key]
    public int Id { get; set; }

    // Identificatorul planului alimentar (ex: "meal-plan-cut")
    [Required]
    [StringLength(100)]
    public string MealPlanIdentifier { get; set; } = string.Empty;

    // Ziua din plan (ex: "day-1", "day-2" ... "day-7")
    [Required]
    [StringLength(20)]
    public string DayId { get; set; } = string.Empty;

    [Required]
    public MealSlot MealSlot { get; set; }

    // many-to-many cu FoodItem prin tabela junction
    public ICollection<MealDayEntryFoodItem> MealDayEntryFoodItems { get; set; } = [];

    // FK -> User
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
