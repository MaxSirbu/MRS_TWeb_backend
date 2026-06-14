using Training_and_Workout_App.Domain.Models.FAQ;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface IFaqAction
{
    Task<List<FaqCategoryDto>> GetAllAsync();
    Task<FaqCategoryDto> CreateCategoryAsync(FaqCategoryUpsertDto dto);
    Task<FaqCategoryDto?> UpdateCategoryAsync(int id, FaqCategoryUpsertDto dto);
    Task<bool> DeleteCategoryAsync(int id);
    Task<FaqQuestionDto?> CreateQuestionAsync(FaqQuestionUpsertDto dto);
    Task<FaqQuestionDto?> UpdateQuestionAsync(int id, FaqQuestionUpsertDto dto);
    Task<bool> DeleteQuestionAsync(int id);
}
