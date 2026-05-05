using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities;

public class QuestionnaireEntry
{
    [Key]
    public int Id { get; set; }

    [Required]
    public bool Skipped { get; set; }

    public Dictionary<string, string>? Answers { get; set; }

    [Required]
    public DateTime CompletedAt { get; set; }

    // FK -> User
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    // FK -> Question
    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;
}
