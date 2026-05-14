using Microsoft.AspNetCore.Mvc;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FoodItemController(IFoodItemService foodItemService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await foodItemService.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await foodItemService.GetByIdAsync(id);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetByCategory(string category)
        => Ok(await foodItemService.GetByCategoryAsync(category));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FoodItemCreateDto dto)
        => Ok(await foodItemService.CreateAsync(dto));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] FoodItemCreateDto dto)
        => Ok(await foodItemService.UpdateAsync(id, dto));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await foodItemService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
