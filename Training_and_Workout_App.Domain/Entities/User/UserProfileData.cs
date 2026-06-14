using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Entities.User;

public class UserProfileData
{
    [Key]
    public int Id { get; set; }

    [Range(0.0, 500.0)]
    public double Weight { get; set; }

    [Range(0.0, 300.0)]
    public double Height { get; set; }

    [Range(0, 150)]
    public int Age { get; set; }

    [StringLength(20)]
    public string Gender { get; set; } = string.Empty;

    [Range(0.0, 200.0)]
    public double Bmi { get; set; }

    [Range(0.0, 10000.0)]
    public double Bmr { get; set; }

    [Range(0.0, 20000.0)]
    public double Tdee { get; set; }

    [Range(0, int.MaxValue)]
    public int Streak { get; set; }

    public DateOnly? LastActiveDate { get; set; }

    [StringLength(500)]
    public string AvatarUrl { get; set; } = string.Empty;

    // FK -> User (one-to-one)
    public int UserId { get; set; }
    public UserData User { get; set; } = null!;
}
