using Training_and_Workout_App.Domain.Entities.Plans;
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

    [Range(1.0, 20.0)]
    public double MetValue { get; set; } = 5.0;

    // Vizibilitate - folosit in panoul admin (AdminExercises.tsx)
    public bool Recommended { get; set; }

    public bool Hidden { get; set; }

    // many-to-many with DayPlan
    public ICollection<DayPlanExerciseData> DayPlanExercises { get; set; } = [];
}
