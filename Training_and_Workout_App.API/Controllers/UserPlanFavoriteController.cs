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
[Route("api/userplanfavorite")]
[Authorize]
public class UserPlanFavoriteController(IUserPlanFavoriteAction favoritesActions) : AppControllerBase
{
    // GET api/userplanfavorite?userId=1
    [HttpGet]
    public async Task<IActionResult> GetByUser([FromQuery] int userId)
    {
        var ownership = CheckOwnership(userId);
        if (ownership is not null) return ownership;
        return Ok(await favoritesActions.GetByUserAsync(userId));
    }

    // POST api/userplanfavorite?userId=1
    [HttpPost]
    public async Task<IActionResult> Add([FromQuery] int userId, [FromBody] UserPlanFavoriteDto dto)
    {
        var ownership = CheckOwnership(userId);
        if (ownership is not null) return ownership;
        return Ok(await favoritesActions.AddAsync(userId, dto));
    }

    // DELETE api/userplanfavorite?userId=1&planType=Workout&planIdentifier=plan-push
    [HttpDelete]
    public async Task<IActionResult> Remove(
        [FromQuery] int userId,
        [FromQuery] PlanType planType,
        [FromQuery] string planIdentifier)
    {
        var ownership = CheckOwnership(userId);
        if (ownership is not null) return ownership;
        var deleted = await favoritesActions.RemoveAsync(userId, planType, planIdentifier);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
