using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities;

public class DayPlan
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Label { get; set; } = string.Empty;

    // FK -> WorkoutPlan
    public Guid WorkoutPlanId { get; set; }
    public WorkoutPlan WorkoutPlan { get; set; } = null!;

    // many-to-many with Exercise
    public ICollection<DayPlanExercise> DayPlanExercises { get; set; } = [];
}
