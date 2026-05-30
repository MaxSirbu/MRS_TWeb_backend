using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/workouttracking")]
[Authorize]
public class WorkoutTrackingController(IWorkoutTrackingActions workoutTrackingActions) : AppControllerBase
{
    // GET api/workouttracking/5
    [HttpGet("{workoutPlanId}")]
    public async Task<IActionResult> GetByPlan(int workoutPlanId)
    {
        var result = await workoutTrackingActions.GetByPlanIdAsync(workoutPlanId);
        if (result is null) return NotFound();
        return Ok(result);
    }

    // PUT api/workouttracking/5/sets
    [HttpPut("{workoutPlanId}/sets")]
    public async Task<IActionResult> SaveSets(int workoutPlanId, [FromBody] List<WorkoutSetDto> sets)
        => Ok(await workoutTrackingActions.SaveSetsAsync(workoutPlanId, sets));

    // PUT api/workouttracking/5/pause
    [HttpPut("{workoutPlanId}/pause")]
    public async Task<IActionResult> SavePause(int workoutPlanId, [FromBody] PauseTimeDto pause)
        => Ok(await workoutTrackingActions.SavePauseAsync(workoutPlanId, pause));

    // DELETE api/workouttracking/5
    [HttpDelete("{workoutPlanId}")]
    public async Task<IActionResult> Delete(int workoutPlanId)
    {
        var deleted = await workoutTrackingActions.DeleteAsync(workoutPlanId);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
