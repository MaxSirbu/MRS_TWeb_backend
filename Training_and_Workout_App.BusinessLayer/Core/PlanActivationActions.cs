using Training_and_Workout_App.Domain.Entities.PlanState;
using Training_and_Workout_App.Domain.Models.PlanActivation;
using Microsoft.EntityFrameworkCore;
using Training_and_Workout_App.DataAccess.Context;

namespace Training_and_Workout_App.BusinessLayer.Core;

public class PlanActivationActions(ApplicationDbContext context)
{
    public async Task<PlanActivationResponseDto?> GetActiveAsync(int userId, PlanType planType)
    {
        var activation = await context.PlanActivations
            .FirstOrDefaultAsync(pa => pa.UserId == userId && pa.PlanType == planType);

        return activation is null ? null : MapToDto(activation);
    }

    public async Task<PlanActivationResponseDto> ActivateAsync(int userId, PlanActivationCreateDto dto)
    {
        // Dacă există deja o activare de același tip → o înlocuim (o ștergem mai întâi)
        var existing = await context.PlanActivations
            .FirstOrDefaultAsync(pa => pa.UserId == userId && pa.PlanType == dto.PlanType);

        if (existing is not null)
            context.PlanActivations.Remove(existing);

        var activation = new PlanActivationData
        {
            PlanType = dto.PlanType,
            PlanIdentifier = dto.PlanIdentifier,
            ActivatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
            TotalDays = dto.TotalDays,
            UserId = userId
        };

        context.PlanActivations.Add(activation);
        await context.SaveChangesAsync();

        return MapToDto(activation);
    }

    public async Task<bool> DeactivateAsync(int userId, PlanType planType)
    {
        var activation = await context.PlanActivations
            .FirstOrDefaultAsync(pa => pa.UserId == userId && pa.PlanType == planType);

        if (activation is null) return false;

        context.PlanActivations.Remove(activation);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<PlanActivationResponseDto> ResetCycleAsync(int userId, PlanType planType)
    {
        var activation = await context.PlanActivations
            .FirstOrDefaultAsync(pa => pa.UserId == userId && pa.PlanType == planType)
            ?? throw new KeyNotFoundException($"No active {planType} plan for user {userId}.");

        activation.LastCycleResetAt = DateOnly.FromDateTime(DateTime.UtcNow);
        await context.SaveChangesAsync();

        return MapToDto(activation);
    }

    private static PlanActivationResponseDto MapToDto(PlanActivationData pa) => new()
    {
        Id = pa.Id,
        PlanType = pa.PlanType,
        PlanIdentifier = pa.PlanIdentifier,
        ActivatedAt = pa.ActivatedAt,
        TotalDays = pa.TotalDays,
        LastCycleResetAt = pa.LastCycleResetAt
    };
}
