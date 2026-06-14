namespace Training_and_Workout_App.Domain.Entities.FAQ;

public class FaqCategoryData
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Icon { get; set; } = "help";
    public int Order { get; set; }
    public ICollection<FaqQuestionData> Questions { get; set; } = [];
}
