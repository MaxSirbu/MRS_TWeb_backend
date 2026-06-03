using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/user/{userId}/profile")]
[Authorize]
public class UserProfileController(IUserProfileActions profileActions) : AppControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(int userId)
    {
        var ownership = CheckOwnership(userId);
        if (ownership is not null) return ownership;
        var result = await profileActions.GetByUserIdAsync(userId);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Upsert(int userId, [FromBody] UserProfileDto dto)
    {
        var ownership = CheckOwnership(userId);
        if (ownership is not null) return ownership;
        var result = await profileActions.UpsertAsync(userId, dto);
        return Ok(result);
    }
}
