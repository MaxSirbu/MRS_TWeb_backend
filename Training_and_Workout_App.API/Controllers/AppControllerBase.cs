using Training_and_Workout_App.BusinessLayer.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Training_and_Workout_App.API.Controllers;

/// <summary>
/// Controller de baza — ofera helper-e comune tuturor controller-elor.
/// </summary>
[ApiController]
public abstract class AppControllerBase : ControllerBase
{
    /// <summary>
    /// Returneaza userId-ul din token-ul JWT al request-ului curent.
    /// </summary>
    protected int GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("UserId claim not found in token.");
        return int.Parse(claim.Value);
    }

    /// <summary>
    /// Returneaza true daca userul autentificat are rolul Admin.
    /// </summary>
    protected bool IsAdmin() => User.IsInRole("Admin");

    /// <summary>
    /// Verifica daca userul curent are acces la resursa userId-ului specificat.
    /// Admin-ul poate accesa orice. User-ul obisnuit — doar propriile date.
    /// Returneaza Forbid() daca nu are voie, null daca are voie.
    /// </summary>
    protected IActionResult? CheckOwnership(int resourceUserId)
    {
        if (!IsAdmin() && GetCurrentUserId() != resourceUserId)
            return Forbid();
        return null;
    }

    protected async Task<IActionResult> ForOwnedUserAsync(
        int resourceUserId,
        Func<Task<IActionResult>> action)
    {
        var ownership = CheckOwnership(resourceUserId);
        return ownership ?? await action();
    }

    protected IActionResult NoContentOrNotFound(bool deleted)
    {
        return deleted ? NoContent() : NotFound();
    }
}
