using System.ComponentModel.DataAnnotations;

namespace Training_and_Workout_App.Domain.Models;

public class PauseTimeDto
{
    [Required]
    [Range(0, 60)]
    public int Minutes { get; set; }

    [Required]
    [Range(0, 59)]
    public int Seconds { get; set; }
}
