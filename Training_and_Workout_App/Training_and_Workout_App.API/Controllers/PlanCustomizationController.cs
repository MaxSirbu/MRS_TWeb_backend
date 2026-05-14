using Microsoft.AspNetCore.Mvc;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlanCustomizationController(IPlanCustomizationService planCustomizationService) : ControllerBase
{
    // GET api/plancustomization?userId=1
    [HttpGet]
    public async Task<IActionResult> GetByUser([FromQuery] int userId)
        => Ok(await planCustomizationService.GetByUserAsync(userId));

    // PUT api/plancustomization?userId=1
    [HttpPut]
    public async Task<IActionResult> Upsert([FromQuery] int userId, [FromBody] PlanCustomizationDto dto)
        => Ok(await planCustomizationService.UpsertAsync(userId, dto));
}
