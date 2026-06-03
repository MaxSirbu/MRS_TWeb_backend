using Training_and_Workout_App.BusinessLayer.Interfaces;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/workouttracking")]
[Authorize]
public class WorkoutTrackingController(IWorkoutTrackingAction workoutTrackingActions) : AppControllerBase
{
    // GET api/workouttracking/5
    [HttpGet("{workoutPlanId}")]
    public async Task<IActionResult> GetByPlan(int workoutPlanId)
    {
        var result = await workoutTrackingActions.GetByPlanIdAsync(workoutPlanId);
        if (result is null) return NotFound();
        return Ok(result);
    }

    // PUT api/workouttracking/5/sets
    [HttpPut("{workoutPlanId}/sets")]
    public async Task<IActionResult> SaveSets(int workoutPlanId, [FromBody] List<WorkoutSetDto> sets)
        => Ok(await workoutTrackingActions.SaveSetsAsync(workoutPlanId, sets));

    // PUT api/workouttracking/5/pause
    [HttpPut("{workoutPlanId}/pause")]
    public async Task<IActionResult> SavePause(int workoutPlanId, [FromBody] PauseTimeDto pause)
        => Ok(await workoutTrackingActions.SavePauseAsync(workoutPlanId, pause));

    // DELETE api/workouttracking/5
    [HttpDelete("{workoutPlanId}")]
    public async Task<IActionResult> Delete(int workoutPlanId)
    {
        var deleted = await workoutTrackingActions.DeleteAsync(workoutPlanId);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
