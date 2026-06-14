using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Models.PlanCustomization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/plancustomization")]
[Authorize]
public class PlanCustomizationController(IPlanCustomizationAction planCustomizationActions) : AppControllerBase
{
    // GET api/plancustomization?userId=1
    [HttpGet]
    public async Task<IActionResult> GetByUser([FromQuery] int userId)
    {
        return await ForOwnedUserAsync(
            userId,
            async () => Ok(await planCustomizationActions.GetByUserAsync(userId)));
    }

    // PUT api/plancustomization?userId=1
    [HttpPut]
    public async Task<IActionResult> Upsert([FromQuery] int userId, [FromBody] PlanCustomizationDto dto)
    {
        return await ForOwnedUserAsync(
            userId,
            async () => Ok(await planCustomizationActions.UpsertAsync(userId, dto)));
    }
}
