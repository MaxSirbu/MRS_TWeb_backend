using System.ComponentModel.DataAnnotations;
using Training_and_Workout_App.Domain.Entities;

namespace Training_and_Workout_App.Domain.Models;

public class ExerciseCreateDto
{
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
}
