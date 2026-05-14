using Microsoft.AspNetCore.Mvc;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkoutPlanController(IWorkoutPlanService workoutPlanService) : ControllerBase
{
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId)
        => Ok(await workoutPlanService.GetByUserIdAsync(userId));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await workoutPlanService.GetByIdAsync(id);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPost("user/{userId}")]
    public async Task<IActionResult> Create(int userId, [FromBody] WorkoutPlanCreateDto dto)
        => Ok(await workoutPlanService.CreateAsync(userId, dto));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] WorkoutPlanCreateDto dto)
        => Ok(await workoutPlanService.UpdateAsync(id, dto));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await workoutPlanService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
