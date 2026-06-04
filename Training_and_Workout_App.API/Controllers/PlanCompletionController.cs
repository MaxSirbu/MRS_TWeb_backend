using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Entities.PlanState;
using Training_and_Workout_App.Domain.Models.PlanCompletion;
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
        return await ForOwnedUserAsync(
            userId,
            async () => Ok(await planCompletionActions.GetByUserAsync(userId, planType)));
    }

    // POST api/plancompletion?userId=1
    [HttpPost]
    public async Task<IActionResult> MarkComplete(
        [FromQuery] int userId,
        [FromBody] PlanCompletionCreateDto dto)
    {
        return await ForOwnedUserAsync(
            userId,
            async () => Ok(await planCompletionActions.MarkCompleteAsync(userId, dto)));
    }

    // DELETE api/plancompletion?userId=1&dayToken=...&dateKey=2025-01-01
    [HttpDelete]
    public async Task<IActionResult> Unmark(
        [FromQuery] int userId,
        [FromQuery] string dayToken,
        [FromQuery] DateOnly dateKey)
    {
        return await ForOwnedUserAsync(
            userId,
            async () => NoContentOrNotFound(await planCompletionActions.UnmarkAsync(userId, dayToken, dateKey)));
    }
}
