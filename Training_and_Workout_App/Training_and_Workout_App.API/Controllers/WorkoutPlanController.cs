using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/workoutplan")]
[Authorize]
public class WorkoutPlanController(IWorkoutPlanActions workoutPlanActions) : AppControllerBase
{
    // GET /api/workoutplan/user/{userId}
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var ownership = CheckOwnership(userId);
        if (ownership is not null) return ownership;
        return Ok(await workoutPlanActions.GetByUserIdAsync(userId));
    }

    // GET /api/workoutplan/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await workoutPlanActions.GetByIdAsync(id));

    // POST /api/workoutplan/user/{userId}
    [HttpPost("user/{userId}")]
    public async Task<IActionResult> Create(int userId, [FromBody] WorkoutPlanCreateDto dto)
    {
        var ownership = CheckOwnership(userId);
        if (ownership is not null) return ownership;
        return Ok(await workoutPlanActions.CreateAsync(userId, dto));
    }

    // PUT /api/workoutplan/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] WorkoutPlanCreateDto dto)
        => Ok(await workoutPlanActions.UpdateAsync(id, dto));

    // DELETE /api/workoutplan/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await workoutPlanActions.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
