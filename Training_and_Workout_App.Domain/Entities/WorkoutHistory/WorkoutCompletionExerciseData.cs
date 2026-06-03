using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities.WorkoutHistory;

public class WorkoutCompletionExerciseData
{
    [Key]
    public int Id { get; set; }

    public int WorkoutCompletionId { get; set; }
    public WorkoutCompletionData WorkoutCompletion { get; set; } = null!;

    public int ExerciseId { get; set; }

    [Required]
    [StringLength(150)]
    public string ExerciseNameSnapshot { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MuscleGroupSnapshot { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Order { get; set; }

    public ICollection<WorkoutCompletionSetData> Sets { get; set; } = [];
}
