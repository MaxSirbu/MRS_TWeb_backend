using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities;

public class Exercise
{
    [Key]
    public Guid Id { get; set; }

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

    // many-to-many with DayPlan
    public ICollection<DayPlanExercise> DayPlanExercises { get; set; } = [];
}
