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
[Route("api/plancompletion")]
[Authorize]
public class PlanCompletionController(IPlanCompletionAction planCompletionActions) : AppControllerBase
{
    // GET api/plancompletion?userId=1&planType=Workout (planType optional)
    [HttpGet]
    public async Task<IActionResult> GetByUser(
        [FromQuery] int userId,
        [FromQuery] PlanType? planType = null)
    {
        var ownership = CheckOwnership(userId);
        if (ownership is not null) return ownership;
        return Ok(await planCompletionActions.GetByUserAsync(userId, planType));
    }

    // POST api/plancompletion?userId=1
    [HttpPost]
    public async Task<IActionResult> MarkComplete(
        [FromQuery] int userId,
        [FromBody] PlanCompletionCreateDto dto)
    {
        var ownership = CheckOwnership(userId);
        if (ownership is not null) return ownership;
        return Ok(await planCompletionActions.MarkCompleteAsync(userId, dto));
    }

    // DELETE api/plancompletion?userId=1&dayToken=...&dateKey=2025-01-01
    [HttpDelete]
    public async Task<IActionResult> Unmark(
        [FromQuery] int userId,
        [FromQuery] string dayToken,
        [FromQuery] DateOnly dateKey)
    {
        var ownership = CheckOwnership(userId);
        if (ownership is not null) return ownership;
        var deleted = await planCompletionActions.UnmarkAsync(userId, dayToken, dateKey);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
