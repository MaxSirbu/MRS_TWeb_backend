using Microsoft.AspNetCore.Mvc;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Entities;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlanCompletionController(IPlanCompletionService planCompletionService) : ControllerBase
{
    // GET api/plancompletion?userId=1&planType=Workout (planType optional)
    [HttpGet]
    public async Task<IActionResult> GetByUser(
        [FromQuery] int userId,
        [FromQuery] PlanType? planType = null)
        => Ok(await planCompletionService.GetByUserAsync(userId, planType));

    // POST api/plancompletion?userId=1
    [HttpPost]
    public async Task<IActionResult> MarkComplete(
        [FromQuery] int userId,
        [FromBody] PlanCompletionCreateDto dto)
        => Ok(await planCompletionService.MarkCompleteAsync(userId, dto));

    // DELETE api/plancompletion?userId=1&dayToken=...&dateKey=2025-01-01
    [HttpDelete]
    public async Task<IActionResult> Unmark(
        [FromQuery] int userId,
        [FromQuery] string dayToken,
        [FromQuery] DateOnly dateKey)
    {
        var deleted = await planCompletionService.UnmarkAsync(userId, dayToken, dateKey);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
