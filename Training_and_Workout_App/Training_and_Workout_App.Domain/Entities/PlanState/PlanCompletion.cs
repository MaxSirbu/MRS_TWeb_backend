using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities;

public class PlanCompletion
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateOnly DateKey { get; set; }

    [Required]
    public PlanType PlanType { get; set; }

    // "planIdentifier:dayId"
    [Required]
    [StringLength(200)]
    public string DayToken { get; set; } = string.Empty;

    // FK -> User
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
