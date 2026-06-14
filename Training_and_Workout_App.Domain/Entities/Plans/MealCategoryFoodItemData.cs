using Training_and_Workout_App.Domain.Entities.FoodItem;
using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities.Plans;

public class MealCategoryFoodItemData
{
    [Key]
    public int Id { get; set; }

    public int MealCategoryId { get; set; }
    public MealCategoryData MealCategory { get; set; } = null!;

    public int FoodItemId { get; set; }
    public FoodItemData FoodItem { get; set; } = null!;

    [Range(0, int.MaxValue)]
    public int Order { get; set; }

    [Range(0.0, 10000.0)]
    public double QuantityGrams { get; set; } = 100;

    [Range(0.0, 100000.0)]
    public double Kcal { get; set; }

    [Range(0.0, 10000.0)]
    public double Protein { get; set; }

    [Range(0.0, 10000.0)]
    public double Carbs { get; set; }

    [Range(0.0, 10000.0)]
    public double Fats { get; set; }
}
