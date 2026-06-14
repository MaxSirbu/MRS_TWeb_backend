namespace Training_and_Workout_App.Domain.Models.FAQ;

public class FaqQuestionDto
{
    public int Id { get; set; }
    public int FaqCategoryId { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public string Icon { get; set; } = "help";
    public int Order { get; set; }
}
