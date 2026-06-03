using Training_and_Workout_App.Domain.Models.Question;
namespace Training_and_Workout_App.Domain.Models.QuestionnaireEntry;

public class QuestionnaireEntryResponseDto
{
    public int Id { get; set; }
    public bool Skipped { get; set; }
    public Dictionary<string, string>? Answers { get; set; }
    public DateTime CompletedAt { get; set; }
    public QuestionResponseDto? Question { get; set; }
}
