using System.ComponentModel.DataAnnotations;
using Training_and_Workout_App.Domain.Entities;

namespace Training_and_Workout_App.Domain.Models;

public class UserPlanFavoriteDto
{
    [Required]
    public PlanType PlanType { get; set; }

    [Required]
    [StringLength(100)]
    public string PlanIdentifier { get; set; } = string.Empty;
}
