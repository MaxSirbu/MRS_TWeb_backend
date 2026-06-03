using Training_and_Workout_App.Domain.Entities.MealDayEntry;
using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities.Plans;

public class MealCategoryData
{
    [Key]
    public int Id { get; set; }

    [Required]
    public MealSlot Slot { get; set; }

    [Range(0, int.MaxValue)]
    public int Order { get; set; }

    public int MealPlanDayId { get; set; }
    public MealPlanDayData MealPlanDay { get; set; } = null!;

    public ICollection<MealCategoryFoodItemData> Items { get; set; } = [];
}
