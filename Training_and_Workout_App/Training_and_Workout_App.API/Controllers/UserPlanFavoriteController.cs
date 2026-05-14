using Microsoft.AspNetCore.Mvc;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Entities;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserPlanFavoriteController(IUserPlanFavoriteService favoritesService) : ControllerBase
{
    // GET api/userplanfavorite?userId=1
    [HttpGet]
    public async Task<IActionResult> GetByUser([FromQuery] int userId)
        => Ok(await favoritesService.GetByUserAsync(userId));

    // POST api/userplanfavorite?userId=1
    [HttpPost]
    public async Task<IActionResult> Add([FromQuery] int userId, [FromBody] UserPlanFavoriteDto dto)
        => Ok(await favoritesService.AddAsync(userId, dto));

    // DELETE api/userplanfavorite?userId=1&planType=Workout&planIdentifier=plan-push
    [HttpDelete]
    public async Task<IActionResult> Remove(
        [FromQuery] int userId,
        [FromQuery] PlanType planType,
        [FromQuery] string planIdentifier)
    {
        var deleted = await favoritesService.RemoveAsync(userId, planType, planIdentifier);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
