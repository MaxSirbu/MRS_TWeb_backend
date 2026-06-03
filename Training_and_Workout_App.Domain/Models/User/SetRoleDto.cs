namespace Training_and_Workout_App.Domain.Models.User;

public class SetRoleDto
{
    /// <summary>Rolul nou: "Admin" sau "User"</summary>
    public string Role { get; set; } = string.Empty;
}
