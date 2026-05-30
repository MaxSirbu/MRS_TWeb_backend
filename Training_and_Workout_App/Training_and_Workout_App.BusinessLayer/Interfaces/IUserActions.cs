using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface IUserActions
{
    Task<UserResponseDto> RegisterAsync(UserRegisterDto dto);
    Task<AuthResponseDto> LoginAsync(UserLoginDto dto);          // returneaza token JWT
    Task<UserResponseDto?> GetUserByIdAsync(int id);
    Task<UserResponseDto?> GetMeAsync(int userId);              // date proprii din token
    Task<UserResponseDto?> UpdateMeAsync(int userId, UserUpdateDto dto); // update fullName/username
    // ── Admin ──────────────────────────────────────────────────────────────────
    Task<List<UserAdminResponseDto>> GetAllAsync();             // toți userii
    Task<UserAdminResponseDto?> SetRoleAsync(int id, SetRoleDto dto);
    Task<UserAdminResponseDto?> ToggleBlockedAsync(int id);
    Task<bool> DeleteAsync(int id);
}
