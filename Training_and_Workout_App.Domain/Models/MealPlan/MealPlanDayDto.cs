using Training_and_Workout_App.Domain.Entities.MealDayEntry;
using Training_and_Workout_App.Domain.Models.FoodItem;
using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Models.MealPlan;

public class MealPlanDayCreateDto
{
    [Required]
    public string Label { get; set; } = string.Empty;

    [Range(1, 365)]
    public int DayNumber { get; set; }

    public List<MealCategoryCreateDto> Categories { get; set; } = [];
}

public class MealCategoryCreateDto
{
    [Required]
    public MealSlot Slot { get; set; }

    [Range(0, int.MaxValue)]
    public int Order { get; set; }

    public List<MealCategoryFoodItemCreateDto> Items { get; set; } = [];
}

public class MealCategoryFoodItemCreateDto
{
    [Required]
    public int FoodItemId { get; set; }

    [Range(0, int.MaxValue)]
    public int Order { get; set; }

    [Range(0.0, 10000.0)]
    public double QuantityGrams { get; set; } = 100;
}

public class MealPlanDayResponseDto
{
    public int Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public int DayNumber { get; set; }
    public List<MealCategoryResponseDto> Categories { get; set; } = [];
}

public class MealCategoryResponseDto
{
    public int Id { get; set; }
    public MealSlot Slot { get; set; }
    public int Order { get; set; }
    public List<MealCategoryFoodItemResponseDto> Items { get; set; } = [];
}

public class MealCategoryFoodItemResponseDto
{
    public int Id { get; set; }
    public int FoodItemId { get; set; }
    public int Order { get; set; }
    public double QuantityGrams { get; set; }
    public double Kcal { get; set; }
    public double Protein { get; set; }
    public double Carbs { get; set; }
    public double Fats { get; set; }
    public FoodItemResponseDto FoodItem { get; set; } = new();
}

public class MealItemQuantityUpdateDto
{
    [Range(0.0, 10000.0)]
    public double QuantityGrams { get; set; }
}
