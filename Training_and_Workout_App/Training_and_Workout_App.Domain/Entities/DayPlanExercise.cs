using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities;

public class DayPlanExercise
{
    // composite PK configured via OnModelCreating
    [Required]
    public int DayPlanId { get; set; }
    public DayPlan DayPlan { get; set; } = null!;

    [Required]
    public int ExerciseId { get; set; }
    public Exercise Exercise { get; set; } = null!;

    [Range(0, int.MaxValue)]
    public int Order { get; set; }
}
