using Microsoft.EntityFrameworkCore;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.DataAccess.Context;
using Training_and_Workout_App.Domain.Entities;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.BusinessLayer.Structure;

public class MealPlanService(ApplicationDbContext context) : IMealPlanService
{
    public async Task<List<MealPlanResponseDto>> GetByUserIdAsync(int userId)
    {
        return await context.MealPlans
            .Where(mp => mp.UserId == userId)
            .Select(mp => new MealPlanResponseDto
            {
                Id = mp.Id,
                Name = mp.Name,
                UpdatedAt = mp.UpdatedAt,
                Meals = mp.Meals
            })
            .ToListAsync();
    }

    public async Task<MealPlanResponseDto?> GetByIdAsync(int id)
    {
        var mp = await context.MealPlans.FindAsync(id);
        if (mp is null) return null;

        return new MealPlanResponseDto
        {
            Id = mp.Id,
            Name = mp.Name,
            UpdatedAt = mp.UpdatedAt,
            Meals = mp.Meals
        };
    }

    public async Task<MealPlanResponseDto> CreateAsync(int userId, MealPlanCreateDto dto)
    {
        var plan = new MealPlan
        {
            Name = dto.Name,
            Meals = dto.Meals,
            UpdatedAt = DateTime.UtcNow,
            UserId = userId
        };

        context.MealPlans.Add(plan);
        await context.SaveChangesAsync();

        return new MealPlanResponseDto
        {
            Id = plan.Id,
            Name = plan.Name,
            UpdatedAt = plan.UpdatedAt,
            Meals = plan.Meals
        };
    }

    public async Task<MealPlanResponseDto> UpdateAsync(int id, MealPlanCreateDto dto)
    {
        var plan = await context.MealPlans.FindAsync(id)
            ?? throw new KeyNotFoundException($"MealPlan {id} not found.");

        plan.Name = dto.Name;
        plan.Meals = dto.Meals;
        plan.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        return new MealPlanResponseDto
        {
            Id = plan.Id,
            Name = plan.Name,
            UpdatedAt = plan.UpdatedAt,
            Meals = plan.Meals
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var plan = await context.MealPlans.FindAsync(id);
        if (plan is null) return false;

        context.MealPlans.Remove(plan);
        await context.SaveChangesAsync();
        return true;
    }
}
