using Training_and_Workout_App.Domain.Entities.User;
using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities.Plans;

public class MealPlanData
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime UpdatedAt { get; set; }

    [Range(1, 20)]
    public int Meals { get; set; }

    // Valori nutritionale per zi (din MockMealPlan in MyPlans.tsx)
    [Range(0, 10000)]
    public int Kcal { get; set; }

    // Procente macronutrienti (0-100)
    [Range(0, 100)]
    public int Carbs { get; set; }

    [Range(0, 100)]
    public int Proteins { get; set; }

    [Range(0, 100)]
    public int Fats { get; set; }

    [StringLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    // FK -> User
    public int UserId { get; set; }
    public UserData User { get; set; } = null!;

    public ICollection<MealPlanDayData> Days { get; set; } = [];
}
