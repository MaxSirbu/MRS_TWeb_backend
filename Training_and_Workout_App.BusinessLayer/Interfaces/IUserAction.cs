using Training_and_Workout_App.Domain.Models.User;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface IUserAction
{
    Task<UserResponseDto> RegisterAsync(UserRegisterDto dto);
    Task<AuthResponseDto> LoginAsync(UserLoginDto dto);
    Task<UserResponseDto?> GetUserByIdAsync(int id);
    Task<UserResponseDto?> GetMeAsync(int userId);
    Task<UserResponseDto?> UpdateMeAsync(int userId, UserUpdateDto dto);
    Task<List<UserAdminResponseDto>> GetAllAsync();
    Task<UserAdminResponseDto?> SetRoleAsync(int id, SetRoleDto dto);
    Task<UserAdminResponseDto?> ToggleBlockedAsync(int id);
    Task<bool> DeleteAsync(int id);
}
