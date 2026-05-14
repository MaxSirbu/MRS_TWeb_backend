using Microsoft.AspNetCore.Mvc;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MealDayEntryController(IMealDayEntryActions mealDayEntryActions) : ControllerBase
{
    // GET api/mealdayentry?userId=1&mealPlanIdentifier=meal-plan-cut&dayId=day-1
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] int userId,
        [FromQuery] string mealPlanIdentifier,
        [FromQuery] string dayId)
        => Ok(await mealDayEntryActions.GetByUserAndDayAsync(userId, mealPlanIdentifier, dayId));

    // PUT api/mealdayentry?userId=1
    [HttpPut]
    public async Task<IActionResult> Upsert(
        [FromQuery] int userId,
        [FromBody] MealDayEntryCreateDto dto)
        => Ok(await mealDayEntryActions.UpsertAsync(userId, dto));

    // DELETE api/mealdayentry/5?userId=1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, [FromQuery] int userId)
    {
        var deleted = await mealDayEntryActions.DeleteAsync(id, userId);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
