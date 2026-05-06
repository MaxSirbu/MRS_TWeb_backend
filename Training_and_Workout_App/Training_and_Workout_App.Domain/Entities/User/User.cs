using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [StringLength(256, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;

    // one-to-many with WorkoutPlan
    public ICollection<WorkoutPlan> WorkoutPlans { get; set; } = [];

    // one-to-many with MealPlan
    public ICollection<MealPlan> MealPlans { get; set; } = [];

    // one-to-many with QuestionnaireEntry
    public ICollection<QuestionnaireEntry> QuestionnaireEntries { get; set; } = [];

    // Adaugă în clasa User existentă:
    [Required]
    [StringLength(256)]
    public string Username { get; set; } = string.Empty; // email/username unic

    public UserRole Role { get; set; } = UserRole.User;

    public bool Blocked { get; set; }

    // one-to-one cu UserProfile
    public UserProfile? UserProfile { get; set; }

    // one-to-many cu FoodItem prin MealDayEntry
    public ICollection<MealDayEntry> MealDayEntries { get; set; } = [];

    public ICollection<PlanActivation> PlanActivations { get; set; } = [];

    public ICollection<PlanCompletion> PlanCompletions { get; set; } = [];

    public ICollection<PlanCustomization> PlanCustomizations { get; set; } = [];

    public ICollection<UserPlanFavorite> PlanFavorites { get; set; } = [];




}
