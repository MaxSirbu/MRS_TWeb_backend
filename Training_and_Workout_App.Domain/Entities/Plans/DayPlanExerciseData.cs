using Training_and_Workout_App.Domain.Entities.Exercise;
using Training_and_Workout_App.Domain.Entities.WorkoutTracking;
using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities.Plans;

public class DayPlanExerciseData
{
    // composite PK configured via OnModelCreating
    [Required]
    public int DayPlanId { get; set; }
    public DayPlanData DayPlan { get; set; } = null!;

    [Required]
    public int ExerciseId { get; set; }
    public ExerciseData Exercise { get; set; } = null!;

    [Range(0, int.MaxValue)]
    public int Order { get; set; }

    public PauseTimeData? PauseTime { get; set; }

    public ICollection<WorkoutSetData> Sets { get; set; } = [];
}
