using Training_and_Workout_App.Domain.Entities.Exercise;
using Training_and_Workout_App.Domain.Models.Exercise;
using Microsoft.EntityFrameworkCore;
using Training_and_Workout_App.DataAccess.Context;

namespace Training_and_Workout_App.BusinessLayer.Core;

public class ExerciseActions(ApplicationDbContext context)
{
    public async Task<List<ExerciseResponseDto>> GetAllAsync()
    {
        return await context.Exercises
            .Where(e => !e.Hidden
                && e.GifUrl != ""
                && e.GifUrl != "string"
                && e.GifUrl != "null"
                && e.GifUrl != "undefined")
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
            .Where(e => !e.Hidden
                && e.MuscleGroup == muscleGroup
                && e.GifUrl != ""
                && e.GifUrl != "string"
                && e.GifUrl != "null"
                && e.GifUrl != "undefined")
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

    public async Task<ExerciseResponseDto?> UpdateAsync(int id, ExerciseCreateDto dto)
    {
        var exercise = await context.Exercises.FindAsync(id);
        if (exercise is null) return null;

        exercise.Name = dto.Name;
        exercise.MuscleGroup = dto.MuscleGroup;
        exercise.GifUrl = dto.GifUrl;
        exercise.Instructions = dto.Instructions;

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

        exercise.Hidden = true;
        await context.SaveChangesAsync();
        return true;
    }
}
