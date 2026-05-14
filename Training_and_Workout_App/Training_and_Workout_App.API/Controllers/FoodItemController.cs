using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FoodItemController(IFoodItemActions foodItemActions) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
        => Ok(await foodItemActions.GetAllAsync());

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await foodItemActions.GetByIdAsync(id);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpGet("category/{category}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByCategory(string category)
        => Ok(await foodItemActions.GetByCategoryAsync(category));

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] FoodItemCreateDto dto)
        => Ok(await foodItemActions.CreateAsync(dto));

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] FoodItemCreateDto dto)
        => Ok(await foodItemActions.UpdateAsync(id, dto));

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await foodItemActions.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
