using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities;

// Tabela junction intre MealDayEntry si FoodItem (many-to-many)
public class MealDayEntryFoodItem
{
    // Cheie primara compusa configurata in OnModelCreating
    public int MealDayEntryId { get; set; }
    public MealDayEntry MealDayEntry { get; set; } = null!;

    public int FoodItemId { get; set; }
    public FoodItem FoodItem { get; set; } = null!;

    [Range(0, int.MaxValue)]
    public int Order { get; set; }
}
