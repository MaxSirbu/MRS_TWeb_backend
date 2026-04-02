using Training_and_Workout_App.Domain.Entities;

namespace Training_and_Workout_App.Domain.Models;

public class ExerciseResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public MuscleGroup MuscleGroup { get; set; }
    public string GifUrl { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;
}
