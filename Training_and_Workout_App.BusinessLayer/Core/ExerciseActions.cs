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
        var normalizedName = dto.Name.Trim();
        var hiddenExercise = await context.Exercises
            .FirstOrDefaultAsync(e => e.Hidden && e.Name.ToLower() == normalizedName.ToLower());

        if (hiddenExercise is not null)
        {
            hiddenExercise.Name = normalizedName;
            hiddenExercise.MuscleGroup = dto.MuscleGroup;
            hiddenExercise.GifUrl = (dto.GifUrl ?? string.Empty).Trim();
            hiddenExercise.Instructions = dto.Instructions.Trim();
            hiddenExercise.Hidden = false;

            await context.SaveChangesAsync();

            return new ExerciseResponseDto
            {
                Id = hiddenExercise.Id,
                Name = hiddenExercise.Name,
                MuscleGroup = hiddenExercise.MuscleGroup,
                GifUrl = hiddenExercise.GifUrl,
                Instructions = hiddenExercise.Instructions
            };
        }

        var exercise = new ExerciseData
        {
            Name = normalizedName,
            MuscleGroup = dto.MuscleGroup,
            GifUrl = (dto.GifUrl ?? string.Empty).Trim(),
            Instructions = dto.Instructions.Trim(),
            Hidden = false
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

        exercise.Name = dto.Name.Trim();
        exercise.MuscleGroup = dto.MuscleGroup;
        exercise.GifUrl = (dto.GifUrl ?? string.Empty).Trim();
        exercise.Instructions = dto.Instructions.Trim();

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
