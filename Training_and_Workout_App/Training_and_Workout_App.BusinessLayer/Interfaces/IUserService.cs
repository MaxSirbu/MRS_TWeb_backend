using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;
public interface IUserService
{
    Task<UserResponseDto> RegisterAsync(UserRegisterDto dto);
    Task<UserResponseDto> LoginAsync(UserLoginDto dto);
    Task<UserResponseDto?> GetUserByIdAsync(int id);
}
