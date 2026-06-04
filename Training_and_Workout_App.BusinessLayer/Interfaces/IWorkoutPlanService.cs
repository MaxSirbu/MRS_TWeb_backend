using Training_and_Workout_App.Domain.Entities.Plans;
using Training_and_Workout_App.Domain.Models.Questionnaire;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface IWorkoutPlanService
{
    Task<WorkoutPlanData> GenerateAsync(
        int userId,
        IReadOnlyDictionary<int, QuestionnaireAnswerDto> answers);
}
