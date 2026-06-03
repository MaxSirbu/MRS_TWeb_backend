namespace Training_and_Workout_App.Domain.Entities.FAQ;

public class FaqQuestionData
{
    public int Id { get; set; }
    public int FaqCategoryId { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public string Icon { get; set; } = "help";
    public int Order { get; set; }

    public FaqCategoryData? Category { get; set; }
}
