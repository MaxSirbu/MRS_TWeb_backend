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

public class ExerciseActions(ApplicationDbContext context)
{
    public async Task<List<ExerciseResponseDto>> GetAllAsync()
    {
        return await context.Exercises
            .Where(e => !e.Hidden)
            .Select(e => new ExerciseResponseDto
            {
                Id = e.Id,
                Name = e.Name,
                MuscleGroup = e.MuscleGroup,
                GifUrl = e.GifUrl,
                Instructions = e.Instructions
            })
            .ToListAsync();
    }

    public async Task<List<ExerciseResponseDto>> GetByMuscleGroupAsync(MuscleGroup muscleGroup)
    {
        return await context.Exercises
            .Where(e => !e.Hidden && e.MuscleGroup == muscleGroup)
            .Select(e => new ExerciseResponseDto
            {
                Id = e.Id,
                Name = e.Name,
                MuscleGroup = e.MuscleGroup,
                GifUrl = e.GifUrl,
                Instructions = e.Instructions
            })
            .ToListAsync();
    }

    public async Task<ExerciseResponseDto?> GetByIdAsync(int id)
    {
        var e = await context.Exercises.FindAsync(id);
        if (e is null) return null;

        return new ExerciseResponseDto
        {
            Id = e.Id,
            Name = e.Name,
            MuscleGroup = e.MuscleGroup,
            GifUrl = e.GifUrl,
            Instructions = e.Instructions
        };
    }

    public async Task<ExerciseResponseDto> CreateAsync(ExerciseCreateDto dto)
    {
        var exercise = new ExerciseData
        {
            Name = dto.Name,
            MuscleGroup = dto.MuscleGroup,
            GifUrl = dto.GifUrl,
            Instructions = dto.Instructions
        };

        context.Exercises.Add(exercise);
        await context.SaveChangesAsync();

        return new ExerciseResponseDto
        {
            Id = exercise.Id,
            Name = exercise.Name,
            MuscleGroup = exercise.MuscleGroup,
            GifUrl = exercise.GifUrl,
            Instructions = exercise.Instructions
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var exercise = await context.Exercises.FindAsync(id);
        if (exercise is null) return false;

        context.Exercises.Remove(exercise);
        await context.SaveChangesAsync();
        return true;
    }
}
