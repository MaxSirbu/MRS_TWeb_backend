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

namespace Training_and_Workout_App.Domain.Entities.Exercise;

public class ExerciseData
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(150, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public MuscleGroup MuscleGroup { get; set; }

    [Required]
    [StringLength(500)]
    public string GifUrl { get; set; } = string.Empty;

    [Required]
    [StringLength(2000, MinimumLength = 1)]
    public string Instructions { get; set; } = string.Empty;

    // Vizibilitate - folosit in panoul admin (AdminExercises.tsx)
    public bool Recommended { get; set; }

    public bool Hidden { get; set; }

    // many-to-many with DayPlan
    public ICollection<DayPlanExerciseData> DayPlanExercises { get; set; } = [];
}
