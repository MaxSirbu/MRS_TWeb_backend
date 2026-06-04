using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Models.UserProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/user/{userId}/profile")]
[Authorize]
public class UserProfileController(IUserProfileAction profileActions) : AppControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(int userId)
    {
        return await ForOwnedUserAsync(userId, async () =>
        {
            var result = await profileActions.GetByUserIdAsync(userId);
            return result is null ? NotFound() : Ok(result);
        });
    }

    [HttpPut]
    public async Task<IActionResult> Upsert(int userId, [FromBody] UserProfileDto dto)
    {
        return await ForOwnedUserAsync(
            userId,
            async () => Ok(await profileActions.UpsertAsync(userId, dto)));
    }
}
