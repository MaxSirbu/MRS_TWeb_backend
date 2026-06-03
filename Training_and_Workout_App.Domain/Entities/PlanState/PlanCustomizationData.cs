using Training_and_Workout_App.Domain.Entities.User;
using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities.PlanState;

public class PlanCustomizationData
{
    [Key]
    public int Id { get; set; }

    [Required]
    public PlanType PlanType { get; set; }

    [Required]
    [StringLength(100)]
    public string PlanIdentifier { get; set; } = string.Empty;

    [StringLength(50)]
    public string ColorId { get; set; } = string.Empty;

    [StringLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    // FK -> User
    public int UserId { get; set; }
    public UserData User { get; set; } = null!;
}
