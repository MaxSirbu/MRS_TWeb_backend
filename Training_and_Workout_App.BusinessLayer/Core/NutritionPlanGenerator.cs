using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.DataAccess.Context;
using Training_and_Workout_App.Domain.Entities.FoodItem;
using Training_and_Workout_App.Domain.Entities.MealDayEntry;
using Training_and_Workout_App.Domain.Entities.Plans;
using Training_and_Workout_App.Domain.Entities.User;
using Training_and_Workout_App.Domain.Models.Questionnaire;
using Microsoft.EntityFrameworkCore;

namespace Training_and_Workout_App.BusinessLayer.Core;

public class NutritionPlanGenerator(ApplicationDbContext context) : INutritionPlanGenerator
{
    private const string GeneratedPlanName = "Personalized Nutrition Plan";

    public async Task<NutritionGenerationResult> GenerateAsync(
        int userId,
        IReadOnlyDictionary<int, QuestionnaireAnswerDto> answers)
    {
        var goal = Answer(answers, 1);
        var activityLevel = Answer(answers, 7);
        var dietType = Answer(answers, 8);
        var mealsAnswer = Answer(answers, 9);
        var priority = Answer(answers, 10);
        var personal = answers.GetValueOrDefault(6)?.Answers ?? [];

        var age = IntValue(personal, "age");
        var gender = personal.GetValueOrDefault("gender") ?? string.Empty;
        var height = DoubleValue(personal, "height");
        var weight = DoubleValue(personal, "weight");

        var heightMeters = Math.Max(height / 100.0, 0.01);
        var bmi = Math.Round(weight / Math.Pow(heightMeters, 2), 2);
        var bmr = Math.Round(CalculateBmr(weight, height, age, gender), 2);
        var tdee = Math.Round(bmr * ActivityMultiplier(activityLevel), 2);
        var calories = Math.Max(1200, (int)Math.Round(AdjustCalories(tdee, goal)));
        var macroSplit = MacroSplitFor(priority);
        var mealsPerDay = MealsPerDay(mealsAnswer);

        await UpsertProfileAsync(userId, age, gender, height, weight, bmi, bmr, tdee);

        var existingPlans = await context.MealPlans
            .Where(plan => plan.UserId == userId && plan.Name == GeneratedPlanName)
            .ToListAsync();
        context.MealPlans.RemoveRange(existingPlans);

        var plan = new MealPlanData
        {
            Name = GeneratedPlanName,
            Description = $"{dietType} plan generated from onboarding questionnaire.",
            UpdatedAt = DateTime.UtcNow,
            Meals = mealsPerDay,
            Kcal = calories,
            Carbs = macroSplit.Carbs,
            Proteins = macroSplit.Protein,
            Fats = macroSplit.Fats,
            UserId = userId
        };

        var foods = await SelectFoodsAsync(dietType);
        for (var dayNumber = 1; dayNumber <= 7; dayNumber++)
        {
            plan.Days.Add(CreateDay(dayNumber, mealsPerDay, calories, macroSplit, foods));
        }

        context.MealPlans.Add(plan);
        await context.SaveChangesAsync();

        return new NutritionGenerationResult
        {
            MealPlan = plan,
            Bmi = bmi,
            Bmr = bmr,
            Tdee = tdee,
            Calories = calories
        };
    }

    private async Task UpsertProfileAsync(
        int userId,
        int age,
        string gender,
        double height,
        double weight,
        double bmi,
        double bmr,
        double tdee)
    {
        var profile = await context.UserProfiles.FirstOrDefaultAsync(item => item.UserId == userId);
        if (profile is null)
        {
            context.UserProfiles.Add(new UserProfileData
            {
                UserId = userId,
                Age = age,
                Gender = gender,
                Height = height,
                Weight = weight,
                Bmi = bmi,
                Bmr = bmr,
                Tdee = tdee
            });
            return;
        }

        profile.Age = age;
        profile.Gender = gender;
        profile.Height = height;
        profile.Weight = weight;
        profile.Bmi = bmi;
        profile.Bmr = bmr;
        profile.Tdee = tdee;
    }

    private async Task<List<FoodItemData>> SelectFoodsAsync(string dietType)
    {
        var foods = await context.FoodItems
            .Where(food => !food.Hidden)
            .OrderByDescending(food => food.Recommended)
            .ThenByDescending(food => food.Priority)
            .ThenBy(food => food.Name)
            .ToListAsync();

        var filtered = dietType switch
        {
            "Vegetarian" => foods.Where(food => !ContainsAny(food, ["chicken", "beef", "pork", "fish", "salmon", "tuna", "turkey", "meat"])).ToList(),
            "Vegan" => foods.Where(food => !ContainsAny(food, ["chicken", "beef", "pork", "fish", "salmon", "tuna", "turkey", "meat", "egg", "milk", "cheese", "yogurt", "whey"])).ToList(),
            "Healthy Balanced Diet" => foods.Where(food => food.Recommended).ToList(),
            _ => foods
        };

        return filtered.Count > 0 ? filtered : foods;
    }

    private static MealPlanDayData CreateDay(
        int dayNumber,
        int mealsPerDay,
        int calories,
        MacroSplit macroSplit,
        IReadOnlyList<FoodItemData> foods)
    {
        var day = new MealPlanDayData
        {
            DayNumber = dayNumber,
            Label = $"Day {dayNumber}"
        };

        if (foods.Count == 0) return day;

        var caloriesPerMeal = calories / (double)mealsPerDay;
        for (var index = 0; index < mealsPerDay; index++)
        {
            var food = foods[(dayNumber + index - 1) % foods.Count];
            var quantity = Math.Clamp(caloriesPerMeal / Math.Max(food.Kcal, 1) * Math.Max(food.Grams, 1), 60, 550);
            var multiplier = quantity / Math.Max(food.Grams, 1);

            var category = new MealCategoryData
            {
                Slot = SlotFor(index, mealsPerDay),
                Order = index
            };

            category.Items.Add(new MealCategoryFoodItemData
            {
                FoodItemId = food.Id,
                Order = 0,
                QuantityGrams = Math.Round(quantity, 2),
                Kcal = Math.Round(food.Kcal * multiplier, 2),
                Protein = Math.Round(food.Protein * multiplier, 2),
                Carbs = Math.Round(food.Carbs * multiplier, 2),
                Fats = Math.Round(food.Fats * multiplier, 2)
            });

            day.Categories.Add(category);
        }

        return day;
    }

    private static string Answer(IReadOnlyDictionary<int, QuestionnaireAnswerDto> answers, int questionId)
        => answers.TryGetValue(questionId, out var dto) && dto.Answers is not null
            ? dto.Answers.GetValueOrDefault("value") ?? string.Empty
            : string.Empty;

    private static bool ContainsAny(FoodItemData food, IReadOnlyList<string> terms)
    {
        var text = $"{food.Name} {food.Category} {food.Description}".ToLowerInvariant();
        return terms.Any(text.Contains);
    }

    private static int IntValue(IReadOnlyDictionary<string, string> values, string key)
        => int.TryParse(values.GetValueOrDefault(key), out var value) ? value : 0;

    private static double DoubleValue(IReadOnlyDictionary<string, string> values, string key)
        => double.TryParse(values.GetValueOrDefault(key), out var value) ? value : 0;

    private static double CalculateBmr(double weight, double height, int age, string gender)
    {
        var baseValue = (10 * weight) + (6.25 * height) - (5 * age);
        return gender.Equals("Female", StringComparison.OrdinalIgnoreCase)
            ? baseValue - 161
            : baseValue + 5;
    }

    private static double ActivityMultiplier(string activityLevel) => activityLevel switch
    {
        "Sedentary" => 1.2,
        "Lightly Active" => 1.375,
        "Moderately Active" => 1.55,
        "Very Active" => 1.725,
        _ => 1.2
    };

    private static double AdjustCalories(double tdee, string goal) => goal switch
    {
        "Weight Loss" => tdee - 500,
        "Muscle Gain" => tdee + 300,
        "Improve Endurance and Stamina" => tdee + 200,
        _ => tdee
    };

    private static MacroSplit MacroSplitFor(string priority) => priority switch
    {
        "Calorie Deficit (Fat Loss)" => new MacroSplit(35, 35, 30),
        "Calorie Surplus (Muscle Gain)" => new MacroSplit(45, 30, 25),
        "Performance and Energy" => new MacroSplit(55, 25, 20),
        _ => new MacroSplit(40, 30, 30)
    };

    private static int MealsPerDay(string mealsAnswer) => mealsAnswer switch
    {
        "3 meals" => 3,
        "4 meals" => 4,
        "5 meals" => 5,
        "6 or more meals" => 6,
        _ => 3
    };

    private static MealSlot SlotFor(int index, int mealsPerDay)
    {
        if (index == 0) return MealSlot.Breakfast;
        if (index == mealsPerDay - 1) return MealSlot.Dinner;
        if (index == 1) return MealSlot.Lunch;
        return MealSlot.Snacks;
    }

    private sealed record MacroSplit(int Carbs, int Protein, int Fats);
}
