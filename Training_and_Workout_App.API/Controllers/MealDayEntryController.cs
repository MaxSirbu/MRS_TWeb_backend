using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Models.MealDayEntry;
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
