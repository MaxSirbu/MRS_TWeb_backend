using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Models.Questionnaire;

public class QuestionnaireCompleteDto
{
    [Required]
    [MinLength(10)]
    public List<QuestionnaireAnswerDto> Entries { get; set; } = [];
}
