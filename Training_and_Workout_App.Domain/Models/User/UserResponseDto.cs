namespace Training_and_Workout_App.Domain.Models.User;

public class UserResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;   // email
}
