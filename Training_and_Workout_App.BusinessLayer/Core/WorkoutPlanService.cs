using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.DataAccess.Context;
using Training_and_Workout_App.Domain.Entities.Exercise;
using Training_and_Workout_App.Domain.Entities.Plans;
using Training_and_Workout_App.Domain.Entities.WorkoutTracking;
using Training_and_Workout_App.Domain.Models.Questionnaire;
using Microsoft.EntityFrameworkCore;

namespace Training_and_Workout_App.BusinessLayer.Core;

public class WorkoutPlanService(ApplicationDbContext context) : IWorkoutPlanService
{
    private const string GeneratedPlanName = "Personalized Workout Plan";

    public async Task<WorkoutPlanData> GenerateAsync(
        int userId,
        IReadOnlyDictionary<int, QuestionnaireAnswerDto> answers)
    {
        var goal = Answer(answers, 1);
        var experience = Answer(answers, 2);
        var availability = Answer(answers, 3);
        var location = Answer(answers, 4);
        var duration = Answer(answers, 5);

        var existingPlans = await context.WorkoutPlans
            .Where(plan => plan.UserId == userId && plan.Name == GeneratedPlanName)
            .ToListAsync();
        context.WorkoutPlans.RemoveRange(existingPlans);

        var exercises = await context.Exercises
            .Where(exercise => !exercise.Hidden)
            .OrderByDescending(exercise => exercise.Recommended)
            .ThenBy(exercise => exercise.Name)
            .ToListAsync();

        var now = DateTime.UtcNow;
        var plan = new WorkoutPlanData
        {
            Name = GeneratedPlanName,
            CreatedAt = now,
            UpdatedAt = now,
            UserId = userId
        };

        var workoutDays = WorkoutDays(availability);
        var exercisesPerDay = ExercisesPerDay(duration);
        var template = TemplateFor(goal, workoutDays);
        var setScheme = SetScheme(experience);

        var workoutDayNumbers = WorkoutDayNumbers(workoutDays);
        var workoutIndex = 0;

        for (var dayNumber = 1; dayNumber <= 7; dayNumber++)
        {
            var day = new DayPlanData
            {
                DayNumber = dayNumber,
                Label = $"Day {dayNumber}",
                IsRestDay = !workoutDayNumbers.Contains(dayNumber)
            };

            if (day.IsRestDay)
            {
                plan.Days.Add(day);
                continue;
            }

            var focus = template[workoutIndex % template.Count];
            var selected = SelectExercises(exercises, focus.Groups, location, exercisesPerDay);
            var order = 0;
            foreach (var exercise in selected)
            {
                var dayExercise = new DayPlanExerciseData
                {
                    ExerciseId = exercise.Id,
                    Order = order++,
                    PauseTime = new PauseTimeData
                    {
                        Minutes = exercise.MuscleGroup == MuscleGroup.Cardio ? 1 : setScheme.PauseMinutes,
                        Seconds = 0
                    }
                };

                for (var setIndex = 0; setIndex < setScheme.Sets; setIndex++)
                {
                    dayExercise.Sets.Add(new WorkoutSetData
                    {
                        Order = setIndex,
                        Weight = 0,
                        Reps = exercise.MuscleGroup == MuscleGroup.Cardio ? setScheme.CardioReps : setScheme.Reps
                    });
                }

                day.DayPlanExercises.Add(dayExercise);
            }

            plan.Days.Add(day);
            workoutIndex++;
        }

        context.WorkoutPlans.Add(plan);
        await context.SaveChangesAsync();
        return plan;
    }

    private static string Answer(IReadOnlyDictionary<int, QuestionnaireAnswerDto> answers, int questionId)
        => answers.TryGetValue(questionId, out var dto) && dto.Answers is not null
            ? dto.Answers.GetValueOrDefault("value") ?? string.Empty
            : string.Empty;

    private static int WorkoutDays(string availability) => availability switch
    {
        "2 days" => 2,
        "3 days" => 3,
        "4-5 days" => 5,
        "6-7 days" => 6,
        _ => 3
    };

    private static HashSet<int> WorkoutDayNumbers(int workoutDays) => workoutDays switch
    {
        2 => [1, 4],
        3 => [1, 3, 5],
        5 => [1, 2, 3, 5, 6],
        6 => [1, 2, 3, 4, 5, 6],
        _ => [1, 3, 5]
    };

    private static int ExercisesPerDay(string duration) => duration switch
    {
        "20-30 minutes" => 3,
        "30-45 minutes" => 4,
        "45-60 minutes" => 5,
        "More than 60 minutes" => 6,
        _ => 4
    };

    private static SetPrescription SetScheme(string experience) => experience switch
    {
        "Beginner (0-6 months)" => new SetPrescription(2, 12, 15, 1),
        "Intermediate (6 months - 2 years)" => new SetPrescription(3, 10, 18, 2),
        "Advanced (2-5 years)" => new SetPrescription(4, 8, 20, 2),
        "Expert (5+ years)" => new SetPrescription(5, 6, 24, 3),
        _ => new SetPrescription(3, 10, 18, 2)
    };

    private static List<WorkoutFocus> TemplateFor(string goal, int workoutDays)
    {
        List<WorkoutFocus> baseTemplate = goal switch
        {
            "Weight Loss" => new List<WorkoutFocus>
            {
                new WorkoutFocus("Full Body + Cardio", [MuscleGroup.Cardio, MuscleGroup.Legs, MuscleGroup.Core, MuscleGroup.Chest]),
                new WorkoutFocus("Metabolic Strength", [MuscleGroup.Back, MuscleGroup.Legs, MuscleGroup.Arms, MuscleGroup.Cardio]),
                new WorkoutFocus("Conditioning Circuit", [MuscleGroup.Cardio, MuscleGroup.Core, MuscleGroup.Back, MuscleGroup.Chest])
            },
            "Muscle Gain" => new List<WorkoutFocus>
            {
                new WorkoutFocus("Push Hypertrophy", [MuscleGroup.Chest, MuscleGroup.Arms, MuscleGroup.Core]),
                new WorkoutFocus("Pull Hypertrophy", [MuscleGroup.Back, MuscleGroup.Arms, MuscleGroup.Core]),
                new WorkoutFocus("Leg Strength", [MuscleGroup.Legs, MuscleGroup.Core]),
                new WorkoutFocus("Upper Volume", [MuscleGroup.Chest, MuscleGroup.Back, MuscleGroup.Arms])
            },
            "Improve Endurance and Stamina" => new List<WorkoutFocus>
            {
                new WorkoutFocus("Cardio Conditioning", [MuscleGroup.Cardio, MuscleGroup.Core, MuscleGroup.Legs]),
                new WorkoutFocus("Functional Circuit", [MuscleGroup.Legs, MuscleGroup.Back, MuscleGroup.Cardio, MuscleGroup.Core]),
                new WorkoutFocus("Stamina Builder", [MuscleGroup.Cardio, MuscleGroup.Chest, MuscleGroup.Arms, MuscleGroup.Core])
            },
            _ => new List<WorkoutFocus>
            {
                new WorkoutFocus("Balanced Strength", [MuscleGroup.Chest, MuscleGroup.Back, MuscleGroup.Legs]),
                new WorkoutFocus("Cardio + Core", [MuscleGroup.Cardio, MuscleGroup.Core, MuscleGroup.Legs]),
                new WorkoutFocus("General Fitness", [MuscleGroup.Back, MuscleGroup.Arms, MuscleGroup.Chest, MuscleGroup.Core])
            }
        };

        while (baseTemplate.Count < workoutDays)
        {
            baseTemplate.Add(baseTemplate[baseTemplate.Count % Math.Max(1, baseTemplate.Count)]);
        }

        return baseTemplate;
    }

    private static List<ExerciseData> SelectExercises(
        List<ExerciseData> exercises,
        IReadOnlyList<MuscleGroup> groups,
        string location,
        int count)
    {
        var allowedGroups = location == "At Home (No Equipment)"
            ? groups.Where(group => group is MuscleGroup.Cardio or MuscleGroup.Core or MuscleGroup.Legs).ToList()
            : groups.ToList();

        if (allowedGroups.Count == 0) allowedGroups = groups.ToList();

        var selected = allowedGroups
            .SelectMany(group => exercises.Where(exercise => exercise.MuscleGroup == group).Take(2))
            .DistinctBy(exercise => exercise.Id)
            .Take(count)
            .ToList();

        if (selected.Count < count)
        {
            selected.AddRange(exercises
                .Where(exercise => selected.All(current => current.Id != exercise.Id))
                .Take(count - selected.Count));
        }

        return selected;
    }

    private sealed record WorkoutFocus(string Label, IReadOnlyList<MuscleGroup> Groups);
    private sealed record SetPrescription(int Sets, int Reps, int CardioReps, int PauseMinutes);
}
