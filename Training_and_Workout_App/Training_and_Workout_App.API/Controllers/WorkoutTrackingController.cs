using Microsoft.AspNetCore.Mvc;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkoutTrackingController(IWorkoutTrackingService workoutTrackingService) : ControllerBase
{
    // GET api/workouttracking/5
    [HttpGet("{workoutPlanId}")]
    public async Task<IActionResult> GetByPlan(int workoutPlanId)
    {
        var result = await workoutTrackingService.GetByPlanIdAsync(workoutPlanId);
        if (result is null) return NotFound();
        return Ok(result);
    }

    // PUT api/workouttracking/5/sets
    [HttpPut("{workoutPlanId}/sets")]
    public async Task<IActionResult> SaveSets(int workoutPlanId, [FromBody] List<WorkoutSetDto> sets)
        => Ok(await workoutTrackingService.SaveSetsAsync(workoutPlanId, sets));

    // PUT api/workouttracking/5/pause
    [HttpPut("{workoutPlanId}/pause")]
    public async Task<IActionResult> SavePause(int workoutPlanId, [FromBody] PauseTimeDto pause)
        => Ok(await workoutTrackingService.SavePauseAsync(workoutPlanId, pause));

    // DELETE api/workouttracking/5
    [HttpDelete("{workoutPlanId}")]
    public async Task<IActionResult> Delete(int workoutPlanId)
    {
        var deleted = await workoutTrackingService.DeleteAsync(workoutPlanId);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
