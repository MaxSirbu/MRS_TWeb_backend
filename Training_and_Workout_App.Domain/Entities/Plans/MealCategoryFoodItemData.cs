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
