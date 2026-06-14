using Training_and_Workout_App.Domain.Entities.PlanState;
using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Models.PlanActivation;

public class PlanActivationCreateDto
{
    [Required]
    public PlanType PlanType { get; set; }

    [Required]
    [StringLength(100)]
    public string PlanIdentifier { get; set; } = string.Empty;

    [Range(1, 365)]
    public int TotalDays { get; set; }
}
