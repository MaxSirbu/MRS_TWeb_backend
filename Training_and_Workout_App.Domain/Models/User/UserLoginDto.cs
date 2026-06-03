using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Models.User;

public class UserLoginDto
{
    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Username { get; set; } = string.Empty;   // email

    [Required]
    [StringLength(256, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;
}
