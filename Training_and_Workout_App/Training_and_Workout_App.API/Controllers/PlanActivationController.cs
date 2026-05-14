using Microsoft.AspNetCore.Mvc;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Entities;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlanActivationController(IPlanActivationService planActivationService) : ControllerBase
{
    // GET api/planactivation/active?userId=1&planType=Workout
    [HttpGet("active")]
    public async Task<IActionResult> GetActive([FromQuery] int userId, [FromQuery] PlanType planType)
    {
        var result = await planActivationService.GetActiveAsync(userId, planType);
        if (result is null) return NotFound();
        return Ok(result);
    }

    // POST api/planactivation?userId=1
    [HttpPost]
    public async Task<IActionResult> Activate([FromQuery] int userId, [FromBody] PlanActivationCreateDto dto)
        => Ok(await planActivationService.ActivateAsync(userId, dto));

    // DELETE api/planactivation?userId=1&planType=Workout
    [HttpDelete]
    public async Task<IActionResult> Deactivate([FromQuery] int userId, [FromQuery] PlanType planType)
    {
        var deleted = await planActivationService.DeactivateAsync(userId, planType);
        if (!deleted) return NotFound();
        return NoContent();
    }

    // POST api/planactivation/reset-cycle?userId=1&planType=Meal
    [HttpPost("reset-cycle")]
    public async Task<IActionResult> ResetCycle([FromQuery] int userId, [FromQuery] PlanType planType)
        => Ok(await planActivationService.ResetCycleAsync(userId, planType));
}
