using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Entities.PlanState;
using Training_and_Workout_App.Domain.Models.UserPlanFavorite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/userplanfavorite")]
[Authorize]
public class UserPlanFavoriteController(IUserPlanFavoriteAction favoritesActions) : AppControllerBase
{
    // GET api/userplanfavorite?userId=1
    [HttpGet]
    public async Task<IActionResult> GetByUser([FromQuery] int userId)
    {
        return await ForOwnedUserAsync(
            userId,
            async () => Ok(await favoritesActions.GetByUserAsync(userId)));
    }

    // POST api/userplanfavorite?userId=1
    [HttpPost]
    public async Task<IActionResult> Add([FromQuery] int userId, [FromBody] UserPlanFavoriteDto dto)
    {
        return await ForOwnedUserAsync(
            userId,
            async () => Ok(await favoritesActions.AddAsync(userId, dto)));
    }

    // DELETE api/userplanfavorite?userId=1&planType=Workout&planIdentifier=plan-push
    [HttpDelete]
    public async Task<IActionResult> Remove(
        [FromQuery] int userId,
        [FromQuery] PlanType planType,
        [FromQuery] string planIdentifier)
    {
        return await ForOwnedUserAsync(
            userId,
            async () => NoContentOrNotFound(
                await favoritesActions.RemoveAsync(userId, planType, planIdentifier)));
    }
}
