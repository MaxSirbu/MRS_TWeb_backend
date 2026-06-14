using Training_and_Workout_App.Domain.Entities.PlanState;
using Training_and_Workout_App.Domain.Models.UserPlanFavorite;
using Microsoft.EntityFrameworkCore;
using Training_and_Workout_App.DataAccess.Context;

namespace Training_and_Workout_App.BusinessLayer.Core;

public class UserPlanFavoriteActions(ApplicationDbContext context)
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
            context.UserPlanFavorites.Add(new UserPlanFavoriteData
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
