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

public class FaqActions(ApplicationDbContext context)
{
    public async Task<List<FaqCategoryDto>> GetAllAsync()
    {
        return await context.FaqCategories
            .Include(category => category.Questions)
            .OrderBy(category => category.Order)
            .ThenBy(category => category.Id)
            .Select(category => MapCategory(category))
            .ToListAsync();
    }

    public async Task<FaqCategoryDto> CreateCategoryAsync(FaqCategoryUpsertDto dto)
    {
        var category = new FaqCategoryData
        {
            Title = dto.Title.Trim(),
            Icon = NormalizeIcon(dto.Icon),
            Order = dto.Order
        };

        context.FaqCategories.Add(category);
        await context.SaveChangesAsync();
        return MapCategory(category);
    }

    public async Task<FaqCategoryDto?> UpdateCategoryAsync(int id, FaqCategoryUpsertDto dto)
    {
        var category = await context.FaqCategories
            .Include(c => c.Questions)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category is null) return null;

        category.Title = dto.Title.Trim();
        category.Icon = NormalizeIcon(dto.Icon);
        category.Order = dto.Order;

        await context.SaveChangesAsync();
        return MapCategory(category);
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await context.FaqCategories.FindAsync(id);
        if (category is null) return false;

        context.FaqCategories.Remove(category);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<FaqQuestionDto?> CreateQuestionAsync(FaqQuestionUpsertDto dto)
    {
        var categoryExists = await context.FaqCategories.AnyAsync(c => c.Id == dto.FaqCategoryId);
        if (!categoryExists) return null;

        var question = new FaqQuestionData
        {
            FaqCategoryId = dto.FaqCategoryId,
            Question = dto.Question.Trim(),
            Answer = dto.Answer.Trim(),
            Icon = NormalizeIcon(dto.Icon),
            Order = dto.Order
        };

        context.FaqQuestions.Add(question);
        await context.SaveChangesAsync();
        return MapQuestion(question);
    }

    public async Task<FaqQuestionDto?> UpdateQuestionAsync(int id, FaqQuestionUpsertDto dto)
    {
        var question = await context.FaqQuestions.FindAsync(id);
        if (question is null) return null;

        var categoryExists = await context.FaqCategories.AnyAsync(c => c.Id == dto.FaqCategoryId);
        if (!categoryExists) return null;

        question.FaqCategoryId = dto.FaqCategoryId;
        question.Question = dto.Question.Trim();
        question.Answer = dto.Answer.Trim();
        question.Icon = NormalizeIcon(dto.Icon);
        question.Order = dto.Order;

        await context.SaveChangesAsync();
        return MapQuestion(question);
    }

    public async Task<bool> DeleteQuestionAsync(int id)
    {
        var question = await context.FaqQuestions.FindAsync(id);
        if (question is null) return false;

        context.FaqQuestions.Remove(question);
        await context.SaveChangesAsync();
        return true;
    }

    private static string NormalizeIcon(string icon)
        => string.IsNullOrWhiteSpace(icon) ? "help" : icon.Trim();

    private static FaqCategoryDto MapCategory(FaqCategoryData category) => new()
    {
        Id = category.Id,
        Title = category.Title,
        Icon = category.Icon,
        Order = category.Order,
        Questions = category.Questions
            .OrderBy(question => question.Order)
            .ThenBy(question => question.Id)
            .Select(MapQuestion)
            .ToList()
    };

    private static FaqQuestionDto MapQuestion(FaqQuestionData question) => new()
    {
        Id = question.Id,
        FaqCategoryId = question.FaqCategoryId,
        Question = question.Question,
        Answer = question.Answer,
        Icon = question.Icon,
        Order = question.Order
    };
}
