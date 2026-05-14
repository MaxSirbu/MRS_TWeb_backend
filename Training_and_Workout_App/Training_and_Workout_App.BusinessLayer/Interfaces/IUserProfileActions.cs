using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface IUserProfileActions
{
    Task<UserProfileDto?> GetByUserIdAsync(int userId);

    // Dacă userul nu are profil → INSERT; dacă are → UPDATE
    Task<UserProfileDto> UpsertAsync(int userId, UserProfileDto dto);
}
