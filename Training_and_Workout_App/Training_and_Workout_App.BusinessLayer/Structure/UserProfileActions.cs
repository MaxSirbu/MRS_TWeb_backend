using Microsoft.EntityFrameworkCore;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.DataAccess.Context;
using Training_and_Workout_App.Domain.Entities;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.BusinessLayer.Structure;

public class UserProfileActions(ApplicationDbContext context) : IUserProfileActions
{
    public async Task<UserProfileDto?> GetByUserIdAsync(int userId)
    {
        var profile = await context.UserProfiles
            .FirstOrDefaultAsync(up => up.UserId == userId);

        if (profile is null) return null;

        return new UserProfileDto
        {
            Weight = profile.Weight,
            Height = profile.Height,
            Age = profile.Age,
            Streak = profile.Streak,
            LastActiveDate = profile.LastActiveDate
        };
    }

    public async Task<UserProfileDto> UpsertAsync(int userId, UserProfileDto dto)
    {
        var existing = await context.UserProfiles
            .FirstOrDefaultAsync(up => up.UserId == userId);

        if (existing is not null)
        {
            // UPDATE — actualizează câmpurile profilului existent
            existing.Weight = dto.Weight;
            existing.Height = dto.Height;
            existing.Age = dto.Age;
            existing.Streak = dto.Streak;
            existing.LastActiveDate = dto.LastActiveDate;
        }
        else
        {
            // INSERT — creează profil nou pentru user
            context.UserProfiles.Add(new UserProfile
            {
                Weight = dto.Weight,
                Height = dto.Height,
                Age = dto.Age,
                Streak = dto.Streak,
                LastActiveDate = dto.LastActiveDate,
                UserId = userId
            });
        }

        await context.SaveChangesAsync();
        return dto;
    }
}
