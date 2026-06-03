using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Entities.Exercise;
using Training_and_Workout_App.Domain.Entities.FAQ;
using Training_and_Workout_App.Domain.Entities.FoodItem;
using Training_and_Workout_App.Domain.Entities.MealDayEntry;
using Training_and_Workout_App.Domain.Entities.Plans;
using Training_and_Workout_App.Domain.Entities.PlanState;
using Training_and_Workout_App.Domain.Entities.Question;
using Training_and_Workout_App.Domain.Entities.QuestionnaireEntry;
using Training_and_Workout_App.Domain.Entities.User;
using Training_and_Workout_App.Domain.Entities.WorkoutHistory;
using Training_and_Workout_App.Domain.Entities.WorkoutTracking;
using Training_and_Workout_App.Domain.Models.DayPlan;
using Training_and_Workout_App.Domain.Models.Exercise;
using Training_and_Workout_App.Domain.Models.FAQ;
using Training_and_Workout_App.Domain.Models.FoodItem;
using Training_and_Workout_App.Domain.Models.MealDayEntry;
using Training_and_Workout_App.Domain.Models.MealPlan;
using Training_and_Workout_App.Domain.Models.PlanActivation;
using Training_and_Workout_App.Domain.Models.PlanCompletion;
using Training_and_Workout_App.Domain.Models.PlanCustomization;
using Training_and_Workout_App.Domain.Models.Question;
using Training_and_Workout_App.Domain.Models.QuestionnaireEntry;
using Training_and_Workout_App.Domain.Models.User;
using Training_and_Workout_App.Domain.Models.UserPlanFavorite;
using Training_and_Workout_App.Domain.Models.UserProfile;
using Training_and_Workout_App.Domain.Models.WorkoutPlan;
using Training_and_Workout_App.Domain.Models.WorkoutTracking;
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
            context.UserProfiles.Add(new UserProfileData
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
