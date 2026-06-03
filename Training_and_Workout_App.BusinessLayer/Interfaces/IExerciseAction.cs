using Training_and_Workout_App.Domain.Entities.Exercise;
using Training_and_Workout_App.Domain.Models.Exercise;
namespace Training_and_Workout_App.BusinessLayer.Interfaces;

public interface IExerciseAction
{
    Task<List<ExerciseResponseDto>> GetAllAsync();
    Task<List<ExerciseResponseDto>> GetByMuscleGroupAsync(MuscleGroup muscleGroup);
    Task<ExerciseResponseDto?> GetByIdAsync(int id);
    Task<ExerciseResponseDto> CreateAsync(ExerciseCreateDto dto);
    Task<bool> DeleteAsync(int id);
}
