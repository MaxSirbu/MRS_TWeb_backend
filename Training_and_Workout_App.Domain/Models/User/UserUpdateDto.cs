using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Models.User;

public class UserUpdateDto
{
    [StringLength(100, MinimumLength = 2)]
    public string FullName { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; } = string.Empty;
}
