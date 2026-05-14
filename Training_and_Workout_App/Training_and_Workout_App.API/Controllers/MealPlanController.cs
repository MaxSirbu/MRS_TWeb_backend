using Microsoft.AspNetCore.Mvc;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MealPlanController(IMealPlanActions mealPlanActions) : ControllerBase
{
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId)
        => Ok(await mealPlanActions.GetByUserIdAsync(userId));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await mealPlanActions.GetByIdAsync(id);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPost("user/{userId}")]
    public async Task<IActionResult> Create(int userId, [FromBody] MealPlanCreateDto dto)
        => Ok(await mealPlanActions.CreateAsync(userId, dto));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] MealPlanCreateDto dto)
        => Ok(await mealPlanActions.UpdateAsync(id, dto));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await mealPlanActions.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
