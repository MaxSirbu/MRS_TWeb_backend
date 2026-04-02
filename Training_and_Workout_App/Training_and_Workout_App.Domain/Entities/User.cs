using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities;

public class User
{
    [Key]
    public Guid Id { get; set; }

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
}
