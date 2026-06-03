using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Entities.Exercise;
using Training_and_Workout_App.Domain.Entities.FAQ;
using Training_and_Workout_App.Domain.Entities.FoodItem;
using Training_and_Workout_App.Domain.Entities.MealDayEntry;
using Training_and_Workout_App.Domain.Entities.Plans;
using Training_and_Workout_App.Domain.Entities.PlanState;
using Training_and_Workout_App.Domain.Entities.Question;
using Training_and_Workout_App.Domain.Entities.QuestionnaireEntry;
using Training_and_Workout_App.Domain.Entities.User;
using Training_and_Workout_App.Domain.Entities.WorkoutHistory;
using Training_and_Workout_App.Domain.Entities.WorkoutTracking;
using Training_and_Workout_App.Domain.Models.DayPlan;
using Training_and_Workout_App.Domain.Models.Exercise;
using Training_and_Workout_App.Domain.Models.FAQ;
using Training_and_Workout_App.Domain.Models.FoodItem;
using Training_and_Workout_App.Domain.Models.MealDayEntry;
using Training_and_Workout_App.Domain.Models.MealPlan;
using Training_and_Workout_App.Domain.Models.PlanActivation;
using Training_and_Workout_App.Domain.Models.PlanCompletion;
using Training_and_Workout_App.Domain.Models.PlanCustomization;
using Training_and_Workout_App.Domain.Models.Question;
using Training_and_Workout_App.Domain.Models.QuestionnaireEntry;
using Training_and_Workout_App.Domain.Models.User;
using Training_and_Workout_App.Domain.Models.UserPlanFavorite;
using Training_and_Workout_App.Domain.Models.UserProfile;
using Training_and_Workout_App.Domain.Models.WorkoutPlan;
using Training_and_Workout_App.Domain.Models.WorkoutTracking;
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
    {
        var plan = await workoutPlanActions.GetByIdAsync(id);
        if (plan is null) return NotFound();
        var ownership = CheckOwnership(plan.UserId);
        if (ownership is not null) return ownership;
        return Ok(await workoutPlanActions.UpdateAsync(id, dto));
    }

    // DELETE /api/workoutplan/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var plan = await workoutPlanActions.GetByIdAsync(id);
        if (plan is null) return NotFound();
        var ownership = CheckOwnership(plan.UserId);
        if (ownership is not null) return ownership;
        var deleted = await workoutPlanActions.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
