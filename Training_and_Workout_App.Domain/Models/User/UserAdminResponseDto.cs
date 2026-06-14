namespace Training_and_Workout_App.Domain.Models.User;

/// <summary>
/// Răspuns cu datele unui user vizibil în panoul de admin.
/// </summary>
public class UserAdminResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool Blocked { get; set; }
}
