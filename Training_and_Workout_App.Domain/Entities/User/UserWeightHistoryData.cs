using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities.User;

public class UserWeightHistoryData
{
    [Key]
    public int Id { get; set; }

    [Range(0.1, 500.0)]
    public double Weight { get; set; }

    public DateTime RecordedAt { get; set; }

    public int UserId { get; set; }
    public UserData User { get; set; } = null!;
}
