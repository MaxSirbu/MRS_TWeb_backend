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
[Route("api/planactivation")]
[Authorize]
public class PlanActivationController(IPlanActivationAction planActivationActions) : AppControllerBase
{
    // GET api/planactivation/active?userId=1&planType=Workout
    [HttpGet("active")]
    public async Task<IActionResult> GetActive([FromQuery] int userId, [FromQuery] PlanType planType)
    {
        var ownership = CheckOwnership(userId);
        if (ownership is not null) return ownership;
        var result = await planActivationActions.GetActiveAsync(userId, planType);
        if (result is null) return NotFound();
        return Ok(result);
    }

    // POST api/planactivation?userId=1
    [HttpPost]
    public async Task<IActionResult> Activate([FromQuery] int userId, [FromBody] PlanActivationCreateDto dto)
    {
        var ownership = CheckOwnership(userId);
        if (ownership is not null) return ownership;
        return Ok(await planActivationActions.ActivateAsync(userId, dto));
    }

    // DELETE api/planactivation?userId=1&planType=Workout
    [HttpDelete]
    public async Task<IActionResult> Deactivate([FromQuery] int userId, [FromQuery] PlanType planType)
    {
        var ownership = CheckOwnership(userId);
        if (ownership is not null) return ownership;
        var deleted = await planActivationActions.DeactivateAsync(userId, planType);
        if (!deleted) return NotFound();
        return NoContent();
    }

    // POST api/planactivation/reset-cycle?userId=1&planType=Meal
    [HttpPost("reset-cycle")]
    public async Task<IActionResult> ResetCycle([FromQuery] int userId, [FromQuery] PlanType planType)
    {
        var ownership = CheckOwnership(userId);
        if (ownership is not null) return ownership;
        return Ok(await planActivationActions.ResetCycleAsync(userId, planType));
    }
}
