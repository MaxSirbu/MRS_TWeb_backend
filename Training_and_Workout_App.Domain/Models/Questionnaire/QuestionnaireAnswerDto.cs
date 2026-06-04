using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Models.Questionnaire;

public class QuestionnaireAnswerDto
{
    [Required]
    public int QuestionId { get; set; }

    public bool Skipped { get; set; }

    public Dictionary<string, string>? Answers { get; set; }
}
