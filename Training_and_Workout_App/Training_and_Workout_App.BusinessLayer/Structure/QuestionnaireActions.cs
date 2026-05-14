using Microsoft.EntityFrameworkCore;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.DataAccess.Context;
using Training_and_Workout_App.Domain.Entities;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.BusinessLayer.Structure;

public class QuestionnaireActions(ApplicationDbContext context) : IQuestionnaireActions
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
        var entry = new QuestionnaireEntry
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
