using Training_and_Workout_App.Domain.Entities.QuestionnaireEntry;
using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities.Question;

public class QuestionData
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(300, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Subtitle { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public List<string> Options { get; set; } = [];

    // one-to-many with QuestionnaireEntry
    public ICollection<QuestionnaireEntryData> QuestionnaireEntries { get; set; } = [];
}
