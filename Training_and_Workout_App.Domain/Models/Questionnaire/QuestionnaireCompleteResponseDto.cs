namespace Training_and_Workout_App.Domain.Models.Questionnaire;

public class QuestionnaireCompleteResponseDto
{
    public bool Completed { get; set; }
    public int WorkoutPlanId { get; set; }
    public int MealPlanId { get; set; }
    public double Bmi { get; set; }
    public double Bmr { get; set; }
    public double Tdee { get; set; }
    public int Calories { get; set; }
}
