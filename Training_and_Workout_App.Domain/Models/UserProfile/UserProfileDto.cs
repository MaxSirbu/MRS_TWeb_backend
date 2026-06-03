using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Models.UserProfile;

public class UserProfileDto
{
    [Range(0.0, 500.0)]
    public double Weight { get; set; }

    [Range(0.0, 300.0)]
    public double Height { get; set; }

    [Range(0, 150)]
    public int Age { get; set; }

    public int Streak { get; set; }

    public DateOnly? LastActiveDate { get; set; }
}
