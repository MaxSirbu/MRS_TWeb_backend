using System.ComponentModel.DataAnnotations;
using Training_and_Workout_App.Domain.Entities;

namespace Training_and_Workout_App.Domain.Models;

public class PlanCompletionCreateDto
{
    [Required]
    public PlanType PlanType { get; set; }

    // Format: "planIdentifier:dayId"
    [Required]
    [StringLength(200)]
    public string DayToken { get; set; } = string.Empty;

    [Required]
    public DateOnly DateKey { get; set; }
}
