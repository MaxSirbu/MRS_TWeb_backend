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
[Route("api/mealdayentry")]
[Authorize]
public class MealDayEntryController(IMealDayEntryAction mealDayEntryActions) : AppControllerBase
{
    // GET api/mealdayentry?userId=1&mealPlanIdentifier=meal-plan-cut&dayId=day-1
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] int userId,
        [FromQuery] string mealPlanIdentifier,
        [FromQuery] string dayId)
    {
        var ownership = CheckOwnership(userId);
        if (ownership is not null) return ownership;
        return Ok(await mealDayEntryActions.GetByUserAndDayAsync(userId, mealPlanIdentifier, dayId));
    }

    // PUT api/mealdayentry?userId=1
    [HttpPut]
    public async Task<IActionResult> Upsert(
        [FromQuery] int userId,
        [FromBody] MealDayEntryCreateDto dto)
    {
        var ownership = CheckOwnership(userId);
        if (ownership is not null) return ownership;
        return Ok(await mealDayEntryActions.UpsertAsync(userId, dto));
    }

    // DELETE api/mealdayentry/5?userId=1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, [FromQuery] int userId)
    {
        var ownership = CheckOwnership(userId);
        if (ownership is not null) return ownership;
        var deleted = await mealDayEntryActions.DeleteAsync(id, userId);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
