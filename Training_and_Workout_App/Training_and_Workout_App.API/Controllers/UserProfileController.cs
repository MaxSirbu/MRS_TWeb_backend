using Microsoft.AspNetCore.Mvc;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/user/{userId}/profile")]
public class UserProfileController(IUserProfileService profileService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(int userId)
    {
        var result = await profileService.GetByUserIdAsync(userId);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Upsert(int userId, [FromBody] UserProfileDto dto)
    {
        var result = await profileService.UpsertAsync(userId, dto);
        return Ok(result);
    }
}
