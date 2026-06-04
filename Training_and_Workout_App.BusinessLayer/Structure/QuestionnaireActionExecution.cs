using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.BusinessLayer.Core;
using Training_and_Workout_App.DataAccess.Context;

namespace Training_and_Workout_App.BusinessLayer.Structure;

public class QuestionnaireActionExecution(
    ApplicationDbContext context,
    IWorkoutPlanService workoutPlanService,
    INutritionPlanService nutritionPlanService)
    : QuestionnaireActions(context, workoutPlanService, nutritionPlanService), IQuestionnaireAction
{
}
