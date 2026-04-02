namespace Training_and_Workout_App.Domain.Models;

public class QuestionResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public List<string> Options { get; set; } = [];
}
