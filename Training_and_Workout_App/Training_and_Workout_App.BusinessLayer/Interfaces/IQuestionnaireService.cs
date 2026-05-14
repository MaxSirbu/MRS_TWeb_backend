using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface IQuestionnaireService
{
    Task<List<QuestionResponseDto>> GetAllQuestionsAsync();
    Task<List<QuestionnaireEntryResponseDto>> GetEntriesByUserAsync(int userId);
    Task<QuestionnaireEntryResponseDto> SubmitAnswerAsync(int userId, QuestionnaireEntryCreateDto dto);
}
