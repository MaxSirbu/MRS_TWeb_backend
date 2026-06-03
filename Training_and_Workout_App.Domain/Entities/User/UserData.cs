using Training_and_Workout_App.Domain.Entities.MealDayEntry;
using Training_and_Workout_App.Domain.Entities.Plans;
using Training_and_Workout_App.Domain.Entities.PlanState;
using Training_and_Workout_App.Domain.Entities.QuestionnaireEntry;
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
