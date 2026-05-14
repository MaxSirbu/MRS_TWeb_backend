using Microsoft.EntityFrameworkCore;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.DataAccess.Context;
using Training_and_Workout_App.Domain.Entities;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.BusinessLayer.Structure;

public class UserPlanFavoriteService(ApplicationDbContext context) : IUserPlanFavoriteService
{
    public async Task<List<UserPlanFavoriteDto>> GetByUserAsync(int userId)
    {
        return await context.UserPlanFavorites
            .Where(f => f.UserId == userId)
            .Select(f => new UserPlanFavoriteDto
            {
                PlanType = f.PlanType,
                PlanIdentifier = f.PlanIdentifier
            })
            .ToListAsync();
    }

    public async Task<UserPlanFavoriteDto> AddAsync(int userId, UserPlanFavoriteDto dto)
    {
        // Verifică dacă planul este deja la favorite (evită duplicate)
        var exists = await context.UserPlanFavorites.AnyAsync(f =>
            f.UserId == userId &&
            f.PlanType == dto.PlanType &&
            f.PlanIdentifier == dto.PlanIdentifier);

        if (!exists)
        {
            context.UserPlanFavorites.Add(new UserPlanFavorite
            {
                PlanType = dto.PlanType,
                PlanIdentifier = dto.PlanIdentifier,
                UserId = userId
            });

            await context.SaveChangesAsync();
        }

        return dto;
    }

    public async Task<bool> RemoveAsync(int userId, PlanType planType, string planIdentifier)
    {
        var favorite = await context.UserPlanFavorites
            .FirstOrDefaultAsync(f =>
                f.UserId == userId &&
                f.PlanType == planType &&
                f.PlanIdentifier == planIdentifier);

        if (favorite is null) return false;

        context.UserPlanFavorites.Remove(favorite);
        await context.SaveChangesAsync();
        return true;
    }
}
