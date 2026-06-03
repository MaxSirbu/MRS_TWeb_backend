using Training_and_Workout_App.Domain.Models.FoodItem;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface IFoodItemAction
{
    Task<List<FoodItemResponseDto>> GetAllAsync();
    Task<List<FoodItemResponseDto>> GetByCategoryAsync(string category);
    Task<FoodItemResponseDto?> GetByIdAsync(int id);
    Task<FoodItemResponseDto> CreateAsync(FoodItemCreateDto dto);
    Task<FoodItemResponseDto> UpdateAsync(int id, FoodItemCreateDto dto);
    Task<bool> DeleteAsync(int id);
}
