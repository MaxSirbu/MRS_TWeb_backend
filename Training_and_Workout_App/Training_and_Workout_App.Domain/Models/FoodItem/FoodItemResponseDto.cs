using Training_and_Workout_App.Domain.Entities;

namespace Training_and_Workout_App.Domain.Models;

public class FoodItemResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Kcal { get; set; }
    public double Protein { get; set; }
    public double Carbs { get; set; }
    public double Fats { get; set; }
    public double Grams { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public MealItemType ItemType { get; set; }
    public bool Recommended { get; set; }
}
