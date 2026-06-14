namespace Training_and_Workout_App.Domain.Models.FAQ;

public class FaqCategoryUpsertDto
{
    public string Title { get; set; } = string.Empty;
    public string Icon { get; set; } = "help";
    public int Order { get; set; }
}
