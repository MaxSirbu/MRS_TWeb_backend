using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Entities.PlanState;
using Training_and_Workout_App.Domain.Models.PlanActivation;
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
        return await ForOwnedUserAsync(userId, async () =>
        {
            var result = await planActivationActions.GetActiveAsync(userId, planType);
            return result is null ? NotFound() : Ok(result);
        });
    }

    // POST api/planactivation?userId=1
    [HttpPost]
    public async Task<IActionResult> Activate([FromQuery] int userId, [FromBody] PlanActivationCreateDto dto)
    {
        return await ForOwnedUserAsync(
            userId,
            async () => Ok(await planActivationActions.ActivateAsync(userId, dto)));
    }

    // DELETE api/planactivation?userId=1&planType=Workout
    [HttpDelete]
    public async Task<IActionResult> Deactivate([FromQuery] int userId, [FromQuery] PlanType planType)
    {
        return await ForOwnedUserAsync(
            userId,
            async () => NoContentOrNotFound(await planActivationActions.DeactivateAsync(userId, planType)));
    }

    // POST api/planactivation/reset-cycle?userId=1&planType=Meal
    [HttpPost("reset-cycle")]
    public async Task<IActionResult> ResetCycle([FromQuery] int userId, [FromQuery] PlanType planType)
    {
        return await ForOwnedUserAsync(
            userId,
            async () => Ok(await planActivationActions.ResetCycleAsync(userId, planType)));
    }
}
