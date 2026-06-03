using Training_and_Workout_App.Domain.Entities.PlanState;
using Training_and_Workout_App.Domain.Models.PlanCustomization;
using Microsoft.EntityFrameworkCore;
using Training_and_Workout_App.DataAccess.Context;

namespace Training_and_Workout_App.BusinessLayer.Core;

public class PlanCustomizationActions(ApplicationDbContext context)
{
    public async Task<List<PlanCustomizationDto>> GetByUserAsync(int userId)
    {
        return await context.PlanCustomizations
            .Where(pc => pc.UserId == userId)
            .Select(pc => new PlanCustomizationDto
            {
                PlanType = pc.PlanType,
                PlanIdentifier = pc.PlanIdentifier,
                ColorId = pc.ColorId,
                ImageUrl = pc.ImageUrl
            })
            .ToListAsync();
    }

    public async Task<PlanCustomizationDto> UpsertAsync(int userId, PlanCustomizationDto dto)
    {
        var existing = await context.PlanCustomizations
            .FirstOrDefaultAsync(pc =>
                pc.UserId == userId &&
                pc.PlanType == dto.PlanType &&
                pc.PlanIdentifier == dto.PlanIdentifier);

        if (existing is not null)
        {
            // Actualizează câmpurile existente
            existing.ColorId = dto.ColorId;
            existing.ImageUrl = dto.ImageUrl;
        }
        else
        {
            // Creează customizare nouă
            context.PlanCustomizations.Add(new PlanCustomizationData
            {
                PlanType = dto.PlanType,
                PlanIdentifier = dto.PlanIdentifier,
                ColorId = dto.ColorId,
                ImageUrl = dto.ImageUrl,
                UserId = userId
            });
        }

        await context.SaveChangesAsync();
        return dto;
    }
}
