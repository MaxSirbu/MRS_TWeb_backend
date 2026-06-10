using Training_and_Workout_App.Domain.Entities.User;
using Training_and_Workout_App.Domain.Models.UserProfile;
using Microsoft.EntityFrameworkCore;
using Training_and_Workout_App.DataAccess.Context;

namespace Training_and_Workout_App.BusinessLayer.Core;

public class UserProfileActions(ApplicationDbContext context)
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
            Gender = profile.Gender,
            Bmi = profile.Bmi,
            Bmr = profile.Bmr,
            Tdee = profile.Tdee,
            Streak = profile.Streak,
            LastActiveDate = profile.LastActiveDate,
            AvatarUrl = profile.AvatarUrl
        };
    }

    public async Task<List<UserWeightHistoryDto>> GetWeightHistoryAsync(int userId)
    {
        return await context.UserWeightHistory
            .Where(entry => entry.UserId == userId)
            .OrderBy(entry => entry.RecordedAt)
            .Select(entry => new UserWeightHistoryDto
            {
                Id = entry.Id,
                Weight = entry.Weight,
                RecordedAt = entry.RecordedAt
            })
            .ToListAsync();
    }

    public async Task<UserProfileDto> UpsertAsync(int userId, UserProfileDto dto)
    {
        var existing = await context.UserProfiles
            .FirstOrDefaultAsync(up => up.UserId == userId);
        var shouldRecordWeight = dto.Weight > 0 &&
            (existing is null || Math.Abs(existing.Weight - dto.Weight) >= 0.01);

        if (existing is not null)
        {
            // UPDATE — actualizează câmpurile profilului existent
            existing.Weight = dto.Weight;
            existing.Height = dto.Height;
            existing.Age = dto.Age;
            existing.Gender = dto.Gender ?? existing.Gender;
            existing.Bmi = dto.Bmi;
            existing.Bmr = dto.Bmr;
            existing.Tdee = dto.Tdee;
            existing.Streak = dto.Streak;
            existing.LastActiveDate = dto.LastActiveDate;
            existing.AvatarUrl = dto.AvatarUrl ?? existing.AvatarUrl;
        }
        else
        {
            // INSERT — creează profil nou pentru user
            context.UserProfiles.Add(new UserProfileData
            {
                Weight = dto.Weight,
                Height = dto.Height,
                Age = dto.Age,
                Gender = dto.Gender ?? string.Empty,
                Bmi = dto.Bmi,
                Bmr = dto.Bmr,
                Tdee = dto.Tdee,
                Streak = dto.Streak,
                LastActiveDate = dto.LastActiveDate,
                AvatarUrl = dto.AvatarUrl ?? string.Empty,
                UserId = userId
            });
        }

        if (shouldRecordWeight)
        {
            context.UserWeightHistory.Add(new UserWeightHistoryData
            {
                UserId = userId,
                Weight = dto.Weight,
                RecordedAt = DateTime.UtcNow
            });
        }

        await context.SaveChangesAsync();
        return dto;
    }
}
