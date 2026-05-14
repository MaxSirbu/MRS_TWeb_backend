using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Training_and_Workout_App.BusinessLayer.Interfaces;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController(IUserActions userActions) : AppControllerBase
{
    // GET /api/user/me — date proprii, orice user autentificat
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userId = GetCurrentUserId();
        var result = await userActions.GetMeAsync(userId);
        if (result is null) return NotFound();
        return Ok(result);
    }

    // GET /api/user/{id} — doar Admin poate vedea datele oricarui user
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await userActions.GetUserByIdAsync(id);
        if (result is null) return NotFound();
        return Ok(result);
    }
}
