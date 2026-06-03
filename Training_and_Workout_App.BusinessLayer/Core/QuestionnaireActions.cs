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

public class QuestionnaireActions(ApplicationDbContext context)
{
    public async Task<List<QuestionResponseDto>> GetAllQuestionsAsync()
    {
        return await context.Questions
            .Select(q => new QuestionResponseDto
            {
                Id = q.Id,
                Title = q.Title,
                Subtitle = q.Subtitle,
                Options = q.Options
            })
            .ToListAsync();
    }

    public async Task<List<QuestionnaireEntryResponseDto>> GetEntriesByUserAsync(int userId)
    {
        return await context.QuestionnaireEntries
            .Where(qe => qe.UserId == userId)
            .Include(qe => qe.Question)
            .Select(qe => new QuestionnaireEntryResponseDto
            {
                Id = qe.Id,
                Skipped = qe.Skipped,
                Answers = qe.Answers,
                CompletedAt = qe.CompletedAt,
                Question = qe.Question == null ? null : new QuestionResponseDto
                {
                    Id = qe.Question.Id,
                    Title = qe.Question.Title,
                    Subtitle = qe.Question.Subtitle,
                    Options = qe.Question.Options
                }
            })
            .ToListAsync();
    }

    public async Task<QuestionnaireEntryResponseDto> SubmitAnswerAsync(
        int userId, QuestionnaireEntryCreateDto dto)
    {
        var entry = new QuestionnaireEntryData
        {
            UserId = userId,
            QuestionId = dto.QuestionId,
            Skipped = dto.Skipped,
            Answers = dto.Answers,
            CompletedAt = DateTime.UtcNow
        };

        context.QuestionnaireEntries.Add(entry);
        await context.SaveChangesAsync();

        // Reload cu Question inclus
        await context.Entry(entry).Reference(e => e.Question).LoadAsync();

        return new QuestionnaireEntryResponseDto
        {
            Id = entry.Id,
            Skipped = entry.Skipped,
            Answers = entry.Answers,
            CompletedAt = entry.CompletedAt,
            Question = entry.Question == null ? null : new QuestionResponseDto
            {
                Id = entry.Question.Id,
                Title = entry.Question.Title,
                Subtitle = entry.Question.Subtitle,
                Options = entry.Question.Options
            }
        };
    }
}
