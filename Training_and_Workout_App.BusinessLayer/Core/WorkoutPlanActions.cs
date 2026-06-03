using Training_and_Workout_App.Domain.Entities.Plans;
using Training_and_Workout_App.Domain.Entities.WorkoutTracking;
using Training_and_Workout_App.Domain.Models.DayPlan;
using Training_and_Workout_App.Domain.Models.Exercise;
using Training_and_Workout_App.Domain.Models.WorkoutPlan;
using Training_and_Workout_App.Domain.Models.WorkoutTracking;
using Microsoft.EntityFrameworkCore;
using Training_and_Workout_App.DataAccess.Context;

namespace Training_and_Workout_App.BusinessLayer.Core;

public class WorkoutPlanActions(ApplicationDbContext context)
{
    public async Task<List<WorkoutPlanResponseDto>> GetByUserIdAsync(int userId)
    {
        return await context.WorkoutPlans
            .Where(wp => wp.UserId == userId)
            .Include(wp => wp.Days)
                .ThenInclude(d => d.DayPlanExercises)
                    .ThenInclude(dpe => dpe.Exercise)
            .Include(wp => wp.Days)
                .ThenInclude(d => d.DayPlanExercises)
                    .ThenInclude(dpe => dpe.Sets)
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
            .Include(wp => wp.Days)
                .ThenInclude(d => d.DayPlanExercises)
                    .ThenInclude(dpe => dpe.Sets)
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
        var plan = new WorkoutPlanData
        {
            Name = dto.Name,
            CreatedAt = now,
            UpdatedAt = now,
            UserId = userId
        };

        int dayNumber = 1;
        foreach (var dayDto in dto.Days)
        {
            plan.Days.Add(CreateDayPlan(dayDto, dayNumber++));
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
                    .ThenInclude(dpe => dpe.Sets)
            .FirstOrDefaultAsync(wp => wp.Id == id)
            ?? throw new KeyNotFoundException($"WorkoutPlan {id} not found.");

        plan.Name = dto.Name;
        plan.UpdatedAt = DateTime.UtcNow;

        context.DayPlans.RemoveRange(plan.Days);

        int dayNumber = 1;
        foreach (var dayDto in dto.Days)
        {
            var day = CreateDayPlan(dayDto, dayNumber++);
            day.WorkoutPlanId = plan.Id;
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

    private static DayPlanData CreateDayPlan(DayPlanCreateDto dayDto, int dayNumber)
    {
        var day = new DayPlanData
        {
            Label = dayDto.Label,
            DayNumber = dayNumber
        };

        var exercises = dayDto.Exercises.Count > 0
            ? dayDto.Exercises.OrderBy(e => e.Order)
            : dayDto.ExerciseIds.Select((exerciseId, index) => new WorkoutDayExerciseCreateDto
            {
                ExerciseId = exerciseId,
                Order = index
            });

        foreach (var exerciseDto in exercises)
        {
            var dayExercise = new DayPlanExerciseData
            {
                ExerciseId = exerciseDto.ExerciseId,
                Order = exerciseDto.Order
            };

            foreach (var setDto in exerciseDto.Sets.Select((set, index) => new { set, index }))
            {
                dayExercise.Sets.Add(new WorkoutSetData
                {
                    Order = setDto.index,
                    Weight = setDto.set.Weight,
                    Reps = setDto.set.Reps
                });
            }

            day.DayPlanExercises.Add(dayExercise);
        }

        return day;
    }

    private static WorkoutPlanResponseDto MapToDto(WorkoutPlanData wp) => new()
    {
        Id = wp.Id,
        UserId = wp.UserId,   // necesar pentru ownership check în controller
        Name = wp.Name,
        CreatedAt = wp.CreatedAt,
        UpdatedAt = wp.UpdatedAt,
        Days = wp.Days
            .OrderBy(d => d.DayNumber)
            .Select(d => new DayPlanResponseDto
            {
                Id = d.Id,
                Label = d.Label,
                DayNumber = d.DayNumber,
                Exercises = d.DayPlanExercises
                    .OrderBy(dpe => dpe.Order)
                    .Select(dpe => new ExerciseResponseDto
                    {
                        Id = dpe.Exercise.Id,
                        Name = dpe.Exercise.Name,
                        MuscleGroup = dpe.Exercise.MuscleGroup,
                        GifUrl = dpe.Exercise.GifUrl,
                        Instructions = dpe.Exercise.Instructions
                    }).ToList(),
                DayExercises = d.DayPlanExercises
                    .OrderBy(dpe => dpe.Order)
                    .Select(dpe => new WorkoutDayExerciseResponseDto
                    {
                        DayPlanId = dpe.DayPlanId,
                        ExerciseId = dpe.ExerciseId,
                        Order = dpe.Order,
                        Exercise = new ExerciseResponseDto
                        {
                            Id = dpe.Exercise.Id,
                            Name = dpe.Exercise.Name,
                            MuscleGroup = dpe.Exercise.MuscleGroup,
                            GifUrl = dpe.Exercise.GifUrl,
                            Instructions = dpe.Exercise.Instructions
                        },
                        Sets = dpe.Sets
                            .OrderBy(s => s.Order)
                            .Select(s => new WorkoutSetDto { Weight = s.Weight, Reps = s.Reps })
                            .ToList()
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
