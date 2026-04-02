using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Models;

public class UserRegisterDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [StringLength(256, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;
}
