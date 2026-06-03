using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Entities.Exercise;
using Training_and_Workout_App.Domain.Entities.FAQ;
using Training_and_Workout_App.Domain.Entities.FoodItem;
using Training_and_Workout_App.Domain.Entities.MealDayEntry;
using Training_and_Workout_App.Domain.Entities.Plans;
using Training_and_Workout_App.Domain.Entities.PlanState;
using Training_and_Workout_App.Domain.Entities.Question;
using Training_and_Workout_App.Domain.Entities.QuestionnaireEntry;
using Training_and_Workout_App.Domain.Entities.User;
using Training_and_Workout_App.Domain.Entities.WorkoutHistory;
using Training_and_Workout_App.Domain.Entities.WorkoutTracking;
using Training_and_Workout_App.Domain.Models.DayPlan;
using Training_and_Workout_App.Domain.Models.Exercise;
using Training_and_Workout_App.Domain.Models.FAQ;
using Training_and_Workout_App.Domain.Models.FoodItem;
using Training_and_Workout_App.Domain.Models.MealDayEntry;
using Training_and_Workout_App.Domain.Models.MealPlan;
using Training_and_Workout_App.Domain.Models.PlanActivation;
using Training_and_Workout_App.Domain.Models.PlanCompletion;
using Training_and_Workout_App.Domain.Models.PlanCustomization;
using Training_and_Workout_App.Domain.Models.Question;
using Training_and_Workout_App.Domain.Models.QuestionnaireEntry;
using Training_and_Workout_App.Domain.Models.User;
using Training_and_Workout_App.Domain.Models.UserPlanFavorite;
using Training_and_Workout_App.Domain.Models.UserProfile;
using Training_and_Workout_App.Domain.Models.WorkoutPlan;
using Training_and_Workout_App.Domain.Models.WorkoutTracking;
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
}
