using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Models;

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

    // PUT /api/user/me — actualizare date proprii (fullName, username)
    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe([FromBody] UserUpdateDto dto)
    {
        var userId = GetCurrentUserId();
        var result = await userActions.UpdateMeAsync(userId, dto);
        if (result is null) return NotFound();
        return Ok(result);
    }

    // GET /api/user/{id} — doar Admin poate vedea datele oricarui user
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await userActions.GetUserByIdAsync(id);
        if (result is null) return NotFound();
        return Ok(result);
    }

    // ── Admin endpoints ───────────────────────────────────────────────────────

    // GET /api/user — toti userii (Admin only)
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
        => Ok(await userActions.GetAllAsync());

    // PUT /api/user/{id}/role — schimba rolul unui user (Admin only)
    [HttpPut("{id:int}/role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SetRole(int id, [FromBody] SetRoleDto dto)
    {
        var result = await userActions.SetRoleAsync(id, dto);
        if (result is null) return NotFound();
        return Ok(result);
    }

    // PUT /api/user/{id}/block — toggle blocare (Admin only)
    [HttpPut("{id:int}/block")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ToggleBlocked(int id)
    {
        var result = await userActions.ToggleBlockedAsync(id);
        if (result is null) return NotFound();
        return Ok(result);
    }

    // DELETE /api/user/{id} — sterge user (Admin only)
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await userActions.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}

