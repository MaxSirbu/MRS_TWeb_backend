using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities.FoodItem;

public class FoodItemData
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(150, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0.0, 10000.0)]
    public double Kcal { get; set; }

    [Required]
    [Range(0.0, 1000.0)]
    public double Protein { get; set; }

    [Required]
    [Range(0.0, 1000.0)]
    public double Carbs { get; set; }

    [Required]
    [Range(0.0, 1000.0)]
    public double Fats { get; set; }

    [Required]
    [Range(0.0, 5000.0)]
    public double Grams { get; set; }

    [StringLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Category { get; set; } = string.Empty;

    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    public MealItemType ItemType { get; set; }

    public bool Recommended { get; set; }

    public bool Hidden { get; set; }

    public int Priority { get; set; }

    public int Popularity { get; set; }
}
