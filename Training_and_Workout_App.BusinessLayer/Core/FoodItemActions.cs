using Training_and_Workout_App.Domain.Entities.FoodItem;
using Training_and_Workout_App.Domain.Models.FoodItem;
using Microsoft.EntityFrameworkCore;
using Training_and_Workout_App.DataAccess.Context;

namespace Training_and_Workout_App.BusinessLayer.Core;

public class FoodItemActions(ApplicationDbContext context)
{
    public async Task<List<FoodItemResponseDto>> GetAllAsync()
    {
        return await context.FoodItems
            .Where(f => !f.Hidden)
            .OrderBy(f => f.Priority)
            .Select(f => MapToDto(f))
            .ToListAsync();
    }

    public async Task<List<FoodItemResponseDto>> GetByCategoryAsync(string category)
    {
        return await context.FoodItems
            .Where(f => !f.Hidden && f.Category == category)
            .OrderBy(f => f.Priority)
            .Select(f => MapToDto(f))
            .ToListAsync();
    }

    public async Task<FoodItemResponseDto?> GetByIdAsync(int id)
    {
        var f = await context.FoodItems.FindAsync(id);
        return f is null ? null : MapToDto(f);
    }

    public async Task<FoodItemResponseDto> CreateAsync(FoodItemCreateDto dto)
    {
        var normalizedName = dto.Name.Trim();
        var hiddenItem = await context.FoodItems
            .FirstOrDefaultAsync(f => f.Hidden && f.Name.ToLower() == normalizedName.ToLower());

        if (hiddenItem is not null)
        {
            ApplyFoodItemDto(hiddenItem, dto, normalizedName);
            hiddenItem.Hidden = false;

            await context.SaveChangesAsync();
            return MapToDto(hiddenItem);
        }

        var item = new FoodItemData
        {
            Hidden = false
        };
        ApplyFoodItemDto(item, dto, normalizedName);

        context.FoodItems.Add(item);
        await context.SaveChangesAsync();
        return MapToDto(item);
    }

    public async Task<FoodItemResponseDto> UpdateAsync(int id, FoodItemCreateDto dto)
    {
        var item = await context.FoodItems.FindAsync(id)
            ?? throw new KeyNotFoundException($"FoodItem {id} not found.");

        ApplyFoodItemDto(item, dto, dto.Name.Trim());

        await context.SaveChangesAsync();
        return MapToDto(item);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await context.FoodItems.FindAsync(id);
        if (item is null) return false;

        item.Hidden = true;
        await context.SaveChangesAsync();
        return true;
    }

    private static void ApplyFoodItemDto(FoodItemData item, FoodItemCreateDto dto, string name)
    {
        item.Name = name;
        item.Kcal = dto.Kcal;
        item.Protein = dto.Protein;
        item.Carbs = dto.Carbs;
        item.Fats = dto.Fats;
        item.Grams = dto.Grams;
        item.ImageUrl = dto.ImageUrl.Trim();
        item.Category = dto.Category.Trim();
        item.Description = dto.Description.Trim();
        item.ItemType = dto.ItemType;
        item.PreparationSteps = string.IsNullOrWhiteSpace(dto.PreparationSteps)
            ? null
            : dto.PreparationSteps.Trim();
        item.Recommended = dto.Recommended;
    }

    private static FoodItemResponseDto MapToDto(FoodItemData f) => new()
    {
        Id = f.Id,
        Name = f.Name,
        Kcal = f.Kcal,
        Protein = f.Protein,
        Carbs = f.Carbs,
        Fats = f.Fats,
        Grams = f.Grams,
        ImageUrl = f.ImageUrl,
        Category = f.Category,
        Description = f.Description,
        ItemType = f.ItemType,
        PreparationSteps = f.PreparationSteps,
        Recommended = f.Recommended
    };
}
