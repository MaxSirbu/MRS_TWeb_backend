using Training_and_Workout_App.Domain.Entities.Plans;
using Training_and_Workout_App.Domain.Entities.User;
using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities.WorkoutHistory;

public class WorkoutCompletionData
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }
    public UserData User { get; set; } = null!;

    public int WorkoutPlanId { get; set; }
    public WorkoutPlanData WorkoutPlan { get; set; } = null!;

    public int WorkoutDayId { get; set; }
    public DayPlanData WorkoutDay { get; set; } = null!;

    public DateOnly ScheduledDate { get; set; }
    public DateTime CompletedAt { get; set; }

    [Range(1, 36500)]
    public int CycleNumber { get; set; }

    [Range(1, 7)]
    public int DayNumber { get; set; }

    [Range(0.0, double.MaxValue)]
    public double TotalVolume { get; set; }

    [Range(0, int.MaxValue)]
    public int TotalSets { get; set; }

    [Range(0, int.MaxValue)]
    public int TotalExercises { get; set; }

    public ICollection<WorkoutCompletionExerciseData> Exercises { get; set; } = [];
}
