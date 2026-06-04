using Training_and_Workout_App.Domain.Entities.Plans;
using Training_and_Workout_App.Domain.Models.Questionnaire;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface INutritionPlanGenerator
{
    Task<NutritionGenerationResult> GenerateAsync(
        int userId,
        IReadOnlyDictionary<int, QuestionnaireAnswerDto> answers);
}

public class NutritionGenerationResult
{
    public MealPlanData MealPlan { get; set; } = null!;
    public double Bmi { get; set; }
    public double Bmr { get; set; }
    public double Tdee { get; set; }
    public int Calories { get; set; }
}
