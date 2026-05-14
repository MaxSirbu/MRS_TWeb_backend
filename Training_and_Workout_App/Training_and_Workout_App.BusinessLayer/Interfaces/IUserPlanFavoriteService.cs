using Training_and_Workout_App.Domain.Entities;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface IUserPlanFavoriteService
{
    Task<List<UserPlanFavoriteDto>> GetByUserAsync(int userId);
    Task<UserPlanFavoriteDto> AddAsync(int userId, UserPlanFavoriteDto dto);
    Task<bool> RemoveAsync(int userId, PlanType planType, string planIdentifier);
}
