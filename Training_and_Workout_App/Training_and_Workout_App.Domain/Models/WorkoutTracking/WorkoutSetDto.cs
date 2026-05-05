using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Models;

public class WorkoutSetDto
{
    [Required]
    [Range(0.0, 1000.0)]
    public double Weight { get; set; }

    [Required]
    [Range(1, 500)]
    public int Reps { get; set; }
}
