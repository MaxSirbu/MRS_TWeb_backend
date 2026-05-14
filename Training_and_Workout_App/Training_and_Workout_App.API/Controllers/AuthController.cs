using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.API.Controllers;

/// <summary>
/// Endpoint-uri publice de autentificare — nu necesita token.
/// </summary>
[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public class AuthController(IUserActions userActions) : ControllerBase
{
    // POST /api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
    {
        var result = await userActions.RegisterAsync(dto);
        return Ok(result);
    }

    // POST /api/auth/login — returneaza AuthResponseDto cu token JWT
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        var result = await userActions.LoginAsync(dto);
        return Ok(result);
    }
}
