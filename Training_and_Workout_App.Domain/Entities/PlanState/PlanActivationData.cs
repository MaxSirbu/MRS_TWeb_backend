using Training_and_Workout_App.Domain.Entities.User;
using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities.PlanState;

public class PlanActivationData
{
    [Key]
    public int Id { get; set; }

    [Required]
    public PlanType PlanType { get; set; }

    [Required]
    [StringLength(100)]
    public string PlanIdentifier { get; set; } = string.Empty;

    [Required]
    public DateOnly ActivatedAt { get; set; }

    [Range(1, 365)]
    public int TotalDays { get; set; }

    public DateOnly? LastCycleResetAt { get; set; }

    // FK -> User
    public int UserId { get; set; }
    public UserData User { get; set; } = null!;
}
