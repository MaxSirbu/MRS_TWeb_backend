using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities;

public class DayPlan
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Label { get; set; } = string.Empty;

    // Ordinea zilei in plan: 1, 2, 3... (corespunde "day-1", "day-2" din frontend)
    [Range(1, 365)]
    public int DayNumber { get; set; }

    // FK -> WorkoutPlan
    public int WorkoutPlanId { get; set; }
    public WorkoutPlan WorkoutPlan { get; set; } = null!;

    // many-to-many with Exercise
    public ICollection<DayPlanExercise> DayPlanExercises { get; set; } = [];
}
