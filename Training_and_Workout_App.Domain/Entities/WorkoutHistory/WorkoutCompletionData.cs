using Training_and_Workout_App.Domain.Entities.Exercise;
using Training_and_Workout_App.Domain.Entities.FAQ;
using Training_and_Workout_App.Domain.Entities.FoodItem;
using Training_and_Workout_App.Domain.Entities.MealDayEntry;
using Training_and_Workout_App.Domain.Entities.Plans;
using Training_and_Workout_App.Domain.Entities.PlanState;
using Training_and_Workout_App.Domain.Entities.Question;
using Training_and_Workout_App.Domain.Entities.QuestionnaireEntry;
using Training_and_Workout_App.Domain.Entities.User;
using Training_and_Workout_App.Domain.Entities.WorkoutHistory;
using Training_and_Workout_App.Domain.Entities.WorkoutTracking;
using Training_and_Workout_App.Domain.Models.DayPlan;
using Training_and_Workout_App.Domain.Models.Exercise;
using Training_and_Workout_App.Domain.Models.FAQ;
using Training_and_Workout_App.Domain.Models.FoodItem;
using Training_and_Workout_App.Domain.Models.MealDayEntry;
using Training_and_Workout_App.Domain.Models.MealPlan;
using Training_and_Workout_App.Domain.Models.PlanActivation;
using Training_and_Workout_App.Domain.Models.PlanCompletion;
using Training_and_Workout_App.Domain.Models.PlanCustomization;
using Training_and_Workout_App.Domain.Models.Question;
using Training_and_Workout_App.Domain.Models.QuestionnaireEntry;
using Training_and_Workout_App.Domain.Models.User;
using Training_and_Workout_App.Domain.Models.UserPlanFavorite;
using Training_and_Workout_App.Domain.Models.UserProfile;
using Training_and_Workout_App.Domain.Models.WorkoutPlan;
using Training_and_Workout_App.Domain.Models.WorkoutTracking;
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
