using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Entities.Exercise;
using Training_and_Workout_App.Domain.Models.Exercise;
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

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] ExerciseCreateDto dto)
    {
        var updated = await exerciseActions.UpdateAsync(id, dto);
        if (updated is null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]   // doar Admin sterge exercitii
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await exerciseActions.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
