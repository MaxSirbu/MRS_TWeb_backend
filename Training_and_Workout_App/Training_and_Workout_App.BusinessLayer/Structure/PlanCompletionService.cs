using Microsoft.EntityFrameworkCore;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.DataAccess.Context;
using Training_and_Workout_App.Domain.Entities;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.BusinessLayer.Structure;

public class PlanCompletionService(ApplicationDbContext context) : IPlanCompletionService
{
    public async Task<List<PlanCompletionResponseDto>> GetByUserAsync(int userId, PlanType? planType = null)
    {
        var query = context.PlanCompletions.Where(pc => pc.UserId == userId);

        if (planType.HasValue)
            query = query.Where(pc => pc.PlanType == planType.Value);

        return await query
            .Select(pc => new PlanCompletionResponseDto
            {
                Id = pc.Id,
                PlanType = pc.PlanType,
                DayToken = pc.DayToken,
                DateKey = pc.DateKey
            })
            .ToListAsync();
    }

    public async Task<PlanCompletionResponseDto> MarkCompleteAsync(int userId, PlanCompletionCreateDto dto)
    {
        // Verifică dacă ziua e deja marcată (evită duplicate)
        var exists = await context.PlanCompletions.AnyAsync(pc =>
            pc.UserId == userId &&
            pc.DayToken == dto.DayToken &&
            pc.DateKey == dto.DateKey);

        if (exists)
            throw new InvalidOperationException($"Day '{dto.DayToken}' already marked as complete for {dto.DateKey}.");

        var completion = new PlanCompletion
        {
            PlanType = dto.PlanType,
            DayToken = dto.DayToken,
            DateKey = dto.DateKey,
            UserId = userId
        };

        context.PlanCompletions.Add(completion);
        await context.SaveChangesAsync();

        return new PlanCompletionResponseDto
        {
            Id = completion.Id,
            PlanType = completion.PlanType,
            DayToken = completion.DayToken,
            DateKey = completion.DateKey
        };
    }

    public async Task<bool> UnmarkAsync(int userId, string dayToken, DateOnly dateKey)
    {
        var completion = await context.PlanCompletions
            .FirstOrDefaultAsync(pc =>
                pc.UserId == userId &&
                pc.DayToken == dayToken &&
                pc.DateKey == dateKey);

        if (completion is null) return false;

        context.PlanCompletions.Remove(completion);
        await context.SaveChangesAsync();
        return true;
    }
}
