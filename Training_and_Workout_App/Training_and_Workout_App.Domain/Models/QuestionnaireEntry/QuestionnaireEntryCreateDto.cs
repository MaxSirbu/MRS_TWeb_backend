using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Models;

public class QuestionnaireEntryCreateDto
{
    [Required]
    public int QuestionId { get; set; }

    [Required]
    public bool Skipped { get; set; }

    public Dictionary<string, string>? Answers { get; set; }
}
