using Microsoft.EntityFrameworkCore;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.DataAccess.Context;
using Training_and_Workout_App.Domain.Entities;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.BusinessLayer.Structure;

public class ExerciseActions(ApplicationDbContext context) : IExerciseActions
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
        var exercise = new Exercise
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
