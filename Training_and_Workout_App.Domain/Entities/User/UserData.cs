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

namespace Training_and_Workout_App.Domain.Entities.User;

public class UserData
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [StringLength(256, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty; // email/username unic

    [Required]
    [StringLength(256, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.User;

    public bool Blocked { get; set; }

    // one-to-one cu UserProfile
    public UserProfileData? UserProfile { get; set; }

    // one-to-many with WorkoutPlan
    public ICollection<WorkoutPlanData> WorkoutPlans { get; set; } = [];

    // one-to-many with MealPlan
    public ICollection<MealPlanData> MealPlans { get; set; } = [];

    // one-to-many with QuestionnaireEntry
    public ICollection<QuestionnaireEntryData> QuestionnaireEntries { get; set; } = [];

    // one-to-many cu MealDayEntry (mesele personalizate per zi)
    public ICollection<MealDayEntryData> MealDayEntries { get; set; } = [];

    // one-to-many cu PlanActivation (planul activ curent)
    public ICollection<PlanActivationData> PlanActivations { get; set; } = [];

    // one-to-many cu PlanCompletion (zilele finalizate)
    public ICollection<PlanCompletionData> PlanCompletions { get; set; } = [];

    // one-to-many cu PlanCustomization (culori/imagini personalizate)
    public ICollection<PlanCustomizationData> PlanCustomizations { get; set; } = [];

    // one-to-many cu UserPlanFavorite (planuri favorite)
    public ICollection<UserPlanFavoriteData> PlanFavorites { get; set; } = [];
}
