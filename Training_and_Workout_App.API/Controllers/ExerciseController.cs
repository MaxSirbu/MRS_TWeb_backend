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
[Route("api/[controller]")]
[Authorize]
public class ExerciseController(IExerciseAction exerciseActions) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]   // catalog public — oricine poate vedea
    public async Task<IActionResult> GetAll()
        => Ok(await exerciseActions.GetAllAsync());

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await exerciseActions.GetByIdAsync(id);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpGet("by-muscle/{muscleGroup}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByMuscleGroup(MuscleGroup muscleGroup)
        => Ok(await exerciseActions.GetByMuscleGroupAsync(muscleGroup));

    [HttpPost]
    [Authorize(Roles = "Admin")]   // doar Admin adauga exercitii in catalog
    public async Task<IActionResult> Create([FromBody] ExerciseCreateDto dto)
        => Ok(await exerciseActions.CreateAsync(dto));

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]   // doar Admin sterge exercitii
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await exerciseActions.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
