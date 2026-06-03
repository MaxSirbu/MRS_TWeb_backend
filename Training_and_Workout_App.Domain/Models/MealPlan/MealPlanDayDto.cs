using Training_and_Workout_App.Domain.Entities.Exercise;
using Training_and_Workout_App.Domain.Entities.FAQ;
using Training_and_Workout_App.Domain.Entities.FoodItem;
using Training_and_Workout_App.Domain.Entities.MealDayEntry;
using Training_and_Workout_App.Domain.Entities.Plans;
using Training_and_Workout_App.Domain.Entities.PlanState;
using Training_and_Workout_App.Domain.Entities.Question;
using Training_and_Workout_App.Domain.Entities.QuestionnaireEntry;
using Training_and_Workout_App.Domain.Entities.User;
using Training_and_Workout_App.Domain.Entities.WorkoutHistory;
using Training_and_Workout_App.Domain.Entities.WorkoutTracking;
using Training_and_Workout_App.Domain.Models.DayPlan;
using Training_and_Workout_App.Domain.Models.Exercise;
using Training_and_Workout_App.Domain.Models.FAQ;
using Training_and_Workout_App.Domain.Models.FoodItem;
using Training_and_Workout_App.Domain.Models.MealDayEntry;
using Training_and_Workout_App.Domain.Models.MealPlan;
using Training_and_Workout_App.Domain.Models.PlanActivation;
using Training_and_Workout_App.Domain.Models.PlanCompletion;
using Training_and_Workout_App.Domain.Models.PlanCustomization;
using Training_and_Workout_App.Domain.Models.Question;
using Training_and_Workout_App.Domain.Models.QuestionnaireEntry;
using Training_and_Workout_App.Domain.Models.User;
using Training_and_Workout_App.Domain.Models.UserPlanFavorite;
using Training_and_Workout_App.Domain.Models.UserProfile;
using Training_and_Workout_App.Domain.Models.WorkoutPlan;
using Training_and_Workout_App.Domain.Models.WorkoutTracking;
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
