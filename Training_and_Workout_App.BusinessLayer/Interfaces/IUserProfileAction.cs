using Training_and_Workout_App.Domain.Models.UserProfile;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface IUserProfileAction
{
    Task<UserProfileDto?> GetByUserIdAsync(int userId);

    // Dacă userul nu are profil → INSERT; dacă are → UPDATE
    Task<UserProfileDto> UpsertAsync(int userId, UserProfileDto dto);
}
