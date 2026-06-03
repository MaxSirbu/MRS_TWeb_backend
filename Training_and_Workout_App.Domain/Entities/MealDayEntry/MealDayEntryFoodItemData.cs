using Training_and_Workout_App.Domain.Entities.FoodItem;
using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities.MealDayEntry;

// Tabela junction intre MealDayEntry si FoodItem (many-to-many)
public class MealDayEntryFoodItemData
{
    // Cheie primara compusa configurata in OnModelCreating
    public int MealDayEntryId { get; set; }
    public MealDayEntryData MealDayEntry { get; set; } = null!;

    public int FoodItemId { get; set; }
    public FoodItemData FoodItem { get; set; } = null!;

    [Range(0, int.MaxValue)]
    public int Order { get; set; }
}
