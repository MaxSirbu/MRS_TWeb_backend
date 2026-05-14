using Microsoft.EntityFrameworkCore;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.DataAccess.Context;
using Training_and_Workout_App.Domain.Entities;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.BusinessLayer.Structure;

public class WorkoutPlanActions(ApplicationDbContext context) : IWorkoutPlanActions
{
    public async Task<List<WorkoutPlanResponseDto>> GetByUserIdAsync(int userId)
    {
        return await context.WorkoutPlans
            .Where(wp => wp.UserId == userId)
            .Include(wp => wp.Days)
                .ThenInclude(d => d.DayPlanExercises)
                    .ThenInclude(dpe => dpe.Exercise)
            .Include(wp => wp.WorkoutTracking)
                .ThenInclude(wt => wt!.Sets)
            .Include(wp => wp.WorkoutTracking)
                .ThenInclude(wt => wt!.PauseTime)
            .Select(wp => MapToDto(wp))
            .ToListAsync();
    }

    public async Task<WorkoutPlanResponseDto?> GetByIdAsync(int id)
    {
        var wp = await context.WorkoutPlans
            .Include(wp => wp.Days)
                .ThenInclude(d => d.DayPlanExercises)
                    .ThenInclude(dpe => dpe.Exercise)
            .Include(wp => wp.WorkoutTracking)
                .ThenInclude(wt => wt!.Sets)
            .Include(wp => wp.WorkoutTracking)
                .ThenInclude(wt => wt!.PauseTime)
            .FirstOrDefaultAsync(wp => wp.Id == id);

        return wp is null ? null : MapToDto(wp);
    }

    public async Task<WorkoutPlanResponseDto> CreateAsync(int userId, WorkoutPlanCreateDto dto)
    {
        var now = DateTime.UtcNow;
        var plan = new WorkoutPlan
        {
            Name = dto.Name,
            CreatedAt = now,
            UpdatedAt = now,
            UserId = userId
        };

        // Adaugă zilele și exercițiile asociate
        int dayNumber = 1;
        foreach (var dayDto in dto.Days)
        {
            var day = new DayPlan
            {
                Label = dayDto.Label,
                DayNumber = dayNumber++
            };

            int order = 0;
            foreach (var exId in dayDto.ExerciseIds)
            {
                day.DayPlanExercises.Add(new DayPlanExercise
                {
                    ExerciseId = exId,
                    Order = order++
                });
            }

            plan.Days.Add(day);
        }

        context.WorkoutPlans.Add(plan);
        await context.SaveChangesAsync();

        // Reload cu include pentru a returna datele complete
        return (await GetByIdAsync(plan.Id))!;
    }

    public async Task<WorkoutPlanResponseDto> UpdateAsync(int id, WorkoutPlanCreateDto dto)
    {
        var plan = await context.WorkoutPlans
            .Include(wp => wp.Days)
                .ThenInclude(d => d.DayPlanExercises)
            .FirstOrDefaultAsync(wp => wp.Id == id)
            ?? throw new KeyNotFoundException($"WorkoutPlan {id} not found.");

        plan.Name = dto.Name;
        plan.UpdatedAt = DateTime.UtcNow;

        // Șterge zilele vechi (cascade va șterge și DayPlanExercise)
        context.DayPlans.RemoveRange(plan.Days);

        int dayNumber = 1;
        foreach (var dayDto in dto.Days)
        {
            var day = new DayPlan
            {
                Label = dayDto.Label,
                DayNumber = dayNumber++,
                WorkoutPlanId = plan.Id
            };

            int order = 0;
            foreach (var exId in dayDto.ExerciseIds)
            {
                day.DayPlanExercises.Add(new DayPlanExercise
                {
                    ExerciseId = exId,
                    Order = order++
                });
            }

            context.DayPlans.Add(day);
        }

        await context.SaveChangesAsync();
        return (await GetByIdAsync(plan.Id))!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var plan = await context.WorkoutPlans.FindAsync(id);
        if (plan is null) return false;

        context.WorkoutPlans.Remove(plan);
        await context.SaveChangesAsync();
        return true;
    }

    // Metodă privată de mapare pentru a nu duplica codul
    private static WorkoutPlanResponseDto MapToDto(WorkoutPlan wp) => new()
    {
        Id = wp.Id,
        Name = wp.Name,
        CreatedAt = wp.CreatedAt,
        UpdatedAt = wp.UpdatedAt,
        Days = wp.Days
            .OrderBy(d => d.DayNumber)
            .Select(d => new DayPlanResponseDto
            {
                Id = d.Id,
                Label = d.Label,
                Exercises = d.DayPlanExercises
                    .OrderBy(dpe => dpe.Order)
                    .Select(dpe => new ExerciseResponseDto
                    {
                        Id = dpe.Exercise.Id,
                        Name = dpe.Exercise.Name,
                        MuscleGroup = dpe.Exercise.MuscleGroup,
                        GifUrl = dpe.Exercise.GifUrl,
                        Instructions = dpe.Exercise.Instructions
                    }).ToList()
            }).ToList(),
        WorkoutTracking = wp.WorkoutTracking is null ? null : new WorkoutTrackingStateResponseDto
        {
            Id = wp.WorkoutTracking.Id,
            Sets = wp.WorkoutTracking.Sets
                .Select(s => new WorkoutSetDto { Weight = s.Weight, Reps = s.Reps })
                .ToList(),
            PauseTime = new PauseTimeDto
            {
                Minutes = wp.WorkoutTracking.PauseTime?.Minutes ?? 0,
                Seconds = wp.WorkoutTracking.PauseTime?.Seconds ?? 0
            }
        }
    };
}
