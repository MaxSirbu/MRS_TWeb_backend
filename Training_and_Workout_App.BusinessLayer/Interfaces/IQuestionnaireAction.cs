using Training_and_Workout_App.Domain.Models.Question;
using Training_and_Workout_App.Domain.Models.QuestionnaireEntry;

namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface IQuestionnaireAction
{
    Task<List<QuestionResponseDto>> GetAllQuestionsAsync();
    Task<List<QuestionnaireEntryResponseDto>> GetEntriesByUserAsync(int userId);
    Task<QuestionnaireEntryResponseDto> SubmitAnswerAsync(int userId, QuestionnaireEntryCreateDto dto);
}
