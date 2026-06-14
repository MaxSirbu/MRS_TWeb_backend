using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Models.WorkoutPlan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/workoutplan")]
[Authorize]
public class WorkoutPlanController(IWorkoutPlanAction workoutPlanActions) : AppControllerBase
{
    // GET /api/workoutplan/user/{userId}
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        return await ForOwnedUserAsync(
            userId,
            async () => Ok(await workoutPlanActions.GetByUserIdAsync(userId)));
    }

    // GET /api/workoutplan/user/{userId}/summary
    [HttpGet("user/{userId}/summary")]
    public async Task<IActionResult> GetSummariesByUser(int userId)
    {
        return await ForOwnedUserAsync(
            userId,
            async () => Ok(await workoutPlanActions.GetSummariesByUserIdAsync(userId)));
    }

    // GET /api/workoutplan/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await workoutPlanActions.GetByIdAsync(id);
        if (result is null) return NotFound();
        return await ForOwnedUserAsync(result.UserId, () => Task.FromResult<IActionResult>(Ok(result)));
    }

    // POST /api/workoutplan/user/{userId}
    [HttpPost("user/{userId}")]
    public async Task<IActionResult> Create(int userId, [FromBody] WorkoutPlanCreateDto dto)
    {
        return await ForOwnedUserAsync(
            userId,
            async () => Ok(await workoutPlanActions.CreateAsync(userId, dto)));
    }

    // PUT /api/workoutplan/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] WorkoutPlanCreateDto dto)
    {
        var plan = await workoutPlanActions.GetByIdAsync(id);
        if (plan is null) return NotFound();
        return await ForOwnedUserAsync(
            plan.UserId,
            async () => Ok(await workoutPlanActions.UpdateAsync(id, dto)));
    }

    // DELETE /api/workoutplan/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var plan = await workoutPlanActions.GetByIdAsync(id);
        if (plan is null) return NotFound();
        return await ForOwnedUserAsync(
            plan.UserId,
            async () => NoContentOrNotFound(await workoutPlanActions.DeleteAsync(id)));
    }
}
