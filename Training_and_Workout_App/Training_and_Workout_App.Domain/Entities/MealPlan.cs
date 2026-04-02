using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities;

public class MealPlan
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateTime UpdatedAt { get; set; }

    [Range(1, 20)]
    public int Meals { get; set; }

    // FK -> User
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
