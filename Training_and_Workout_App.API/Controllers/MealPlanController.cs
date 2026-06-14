using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Models.MealPlan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/mealplan")]
[Authorize]
public class MealPlanController(IMealPlanAction mealPlanActions) : AppControllerBase
{
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        return await ForOwnedUserAsync(
            userId,
            async () => Ok(await mealPlanActions.GetByUserIdAsync(userId)));
    }

    [HttpGet("user/{userId}/summary")]
    public async Task<IActionResult> GetSummariesByUser(int userId)
    {
        return await ForOwnedUserAsync(
            userId,
            async () => Ok(await mealPlanActions.GetSummariesByUserIdAsync(userId)));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await mealPlanActions.GetByIdAsync(id);
        if (result is null) return NotFound();
        return await ForOwnedUserAsync(result.UserId, () => Task.FromResult<IActionResult>(Ok(result)));
    }

    [HttpPost("user/{userId}")]
    public async Task<IActionResult> Create(int userId, [FromBody] MealPlanCreateDto dto)
    {
        return await ForOwnedUserAsync(
            userId,
            async () => Ok(await mealPlanActions.CreateAsync(userId, dto)));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] MealPlanCreateDto dto)
    {
        var plan = await mealPlanActions.GetByIdAsync(id);
        if (plan is null) return NotFound();
        return await ForOwnedUserAsync(
            plan.UserId,
            async () => Ok(await mealPlanActions.UpdateAsync(id, dto)));
    }

    [HttpPut("item/{itemId}/quantity")]
    public async Task<IActionResult> UpdateItemQuantity(int itemId, [FromBody] MealItemQuantityUpdateDto dto)
    {
        var item = await mealPlanActions.UpdateItemQuantityAsync(itemId, GetCurrentUserId(), IsAdmin(), dto);
        if (item is null) return NotFound();
        return Ok(item);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var plan = await mealPlanActions.GetByIdAsync(id);
        if (plan is null) return NotFound();
        return await ForOwnedUserAsync(
            plan.UserId,
            async () => NoContentOrNotFound(await mealPlanActions.DeleteAsync(id)));
    }
}
