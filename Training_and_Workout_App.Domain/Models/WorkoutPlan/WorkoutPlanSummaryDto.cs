namespace Training_and_Workout_App.Domain.Models.WorkoutPlan;

public class WorkoutPlanSummaryDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int DayCount { get; set; }
    public int ExerciseCount { get; set; }
    public List<WorkoutPlanDaySummaryDto> Days { get; set; } = [];
}

public class WorkoutPlanDaySummaryDto
{
    public int DayNumber { get; set; }
    public int ExerciseCount { get; set; }
    public double TotalWeight { get; set; }
}
