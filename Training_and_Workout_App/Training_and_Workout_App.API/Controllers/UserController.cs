using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Training_and_Workout_App.BusinessLayer.Interfaces;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]   // doar Admin poate vedea datele oricarui user
public class UserController(IUserActions userActions) : ControllerBase
{
    // GET /api/user/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await userActions.GetUserByIdAsync(id);
        if (result is null) return NotFound();
        return Ok(result);
    }
}

