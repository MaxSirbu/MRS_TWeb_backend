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
        var item = new FoodItemData
        {
            Name = dto.Name,
            Kcal = dto.Kcal,
            Protein = dto.Protein,
            Carbs = dto.Carbs,
            Fats = dto.Fats,
            Grams = dto.Grams,
            ImageUrl = dto.ImageUrl,
            Category = dto.Category,
            Description = dto.Description,
            ItemType = dto.ItemType,
            Recommended = dto.Recommended
        };

        context.FoodItems.Add(item);
        await context.SaveChangesAsync();
        return MapToDto(item);
    }

    public async Task<FoodItemResponseDto> UpdateAsync(int id, FoodItemCreateDto dto)
    {
        var item = await context.FoodItems.FindAsync(id)
            ?? throw new KeyNotFoundException($"FoodItem {id} not found.");

        item.Name = dto.Name;
        item.Kcal = dto.Kcal;
        item.Protein = dto.Protein;
        item.Carbs = dto.Carbs;
        item.Fats = dto.Fats;
        item.Grams = dto.Grams;
        item.ImageUrl = dto.ImageUrl;
        item.Category = dto.Category;
        item.Description = dto.Description;
        item.ItemType = dto.ItemType;
        item.Recommended = dto.Recommended;

        await context.SaveChangesAsync();
        return MapToDto(item);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await context.FoodItems.FindAsync(id);
        if (item is null) return false;

        context.FoodItems.Remove(item);
        await context.SaveChangesAsync();
        return true;
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
        Recommended = f.Recommended
    };
}
