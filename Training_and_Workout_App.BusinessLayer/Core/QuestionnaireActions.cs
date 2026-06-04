using Training_and_Workout_App.Domain.Entities.QuestionnaireEntry;
using Training_and_Workout_App.Domain.Models.Question;
using Training_and_Workout_App.Domain.Models.Questionnaire;
using Training_and_Workout_App.Domain.Models.QuestionnaireEntry;
using Microsoft.EntityFrameworkCore;
using Training_and_Workout_App.DataAccess.Context;
using Training_and_Workout_App.BusinessLayer.Interfaces;

namespace Training_and_Workout_App.BusinessLayer.Core;

public class QuestionnaireActions(
    ApplicationDbContext context,
    IWorkoutPlanService workoutPlanService,
    INutritionPlanService nutritionPlanService)
{
    public async Task<List<QuestionResponseDto>> GetAllQuestionsAsync()
    {
        return await context.Questions
            .Where(q => q.Id >= 1 && q.Id <= 10)
            .OrderBy(q => q.Id)
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

    public async Task<QuestionnaireCompleteResponseDto> CompleteAsync(int userId, QuestionnaireCompleteDto dto)
    {
        var answers = dto.Entries
            .GroupBy(entry => entry.QuestionId)
            .ToDictionary(group => group.Key, group => group.Last());

        ValidateCompleteAnswers(answers);

        var existingEntries = await context.QuestionnaireEntries
            .Where(entry => entry.UserId == userId)
            .ToListAsync();

        context.QuestionnaireEntries.RemoveRange(existingEntries);

        foreach (var answer in answers.Values.OrderBy(answer => answer.QuestionId))
        {
            context.QuestionnaireEntries.Add(new QuestionnaireEntryData
            {
                UserId = userId,
                QuestionId = answer.QuestionId,
                Skipped = false,
                Answers = answer.Answers,
                CompletedAt = DateTime.UtcNow
            });
        }

        await context.SaveChangesAsync();

        var nutrition = await nutritionPlanService.GenerateAsync(userId, answers);
        var workout = await workoutPlanService.GenerateAsync(userId, answers);

        return new QuestionnaireCompleteResponseDto
        {
            Completed = true,
            WorkoutPlanId = workout.Id,
            MealPlanId = nutrition.MealPlan.Id,
            Bmi = nutrition.Bmi,
            Bmr = nutrition.Bmr,
            Tdee = nutrition.Tdee,
            Calories = nutrition.Calories
        };
    }

    private static void ValidateCompleteAnswers(IReadOnlyDictionary<int, QuestionnaireAnswerDto> answers)
    {
        for (var questionId = 1; questionId <= 10; questionId++)
        {
            if (!answers.ContainsKey(questionId))
            {
                throw new InvalidOperationException("All 10 questionnaire questions must be completed.");
            }
        }

        foreach (var questionId in Enumerable.Range(1, 10).Where(id => id != 6))
        {
            var value = answers[questionId].Answers?.GetValueOrDefault("value");
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException($"Question {questionId} requires one selected answer.");
            }
        }

        var personal = answers[6].Answers;
        if (personal is null ||
            !personal.ContainsKey("age") ||
            !personal.ContainsKey("gender") ||
            !personal.ContainsKey("height") ||
            !personal.ContainsKey("weight"))
        {
            throw new InvalidOperationException("Personal information requires age, gender, height, and weight.");
        }

        if (!int.TryParse(personal["age"], out var age) || age <= 0 ||
            !double.TryParse(personal["height"], out var height) || height <= 0 ||
            !double.TryParse(personal["weight"], out var weight) || weight <= 0 ||
            string.IsNullOrWhiteSpace(personal["gender"]))
        {
            throw new InvalidOperationException("Personal information contains invalid values.");
        }
    }
}
