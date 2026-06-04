using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Training_and_Workout_App.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddOnboardingQuestionnaireGeneration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Bmi",
                table: "UserProfiles",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Bmr",
                table: "UserProfiles",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "UserProfiles",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Tdee",
                table: "UserProfiles",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Options", "Subtitle", "Title" },
                values: new object[,]
                {
                    { 1, "[\"Weight Loss\",\"Muscle Gain\",\"Maintain Current Fitness Level\",\"Improve Endurance and Stamina\"]", "Choose the outcome you want your plan to optimize for.", "What is your primary fitness goal?" },
                    { 2, "[\"Beginner (0-6 months)\",\"Intermediate (6 months - 2 years)\",\"Advanced (2-5 years)\",\"Expert (5\\u002B years)\"]", "This helps tune difficulty, volume, and exercise complexity.", "What is your fitness experience level?" },
                    { 3, "[\"2 days\",\"3 days\",\"4-5 days\",\"6-7 days\"]", "Your weekly availability shapes the plan schedule.", "How many days per week can you work out?" },
                    { 4, "[\"At Home (No Equipment)\",\"At Home (With Equipment)\",\"Gym\",\"Both Home and Gym\"]", "Your plan will adapt exercise selection to your training environment.", "Where do you usually train?" },
                    { 5, "[\"20-30 minutes\",\"30-45 minutes\",\"45-60 minutes\",\"More than 60 minutes\"]", "Workout length controls how many movements and sets are prescribed.", "How much time can you dedicate to each workout?" },
                    { 6, "[]", "Enter your data for BMI, BMR, TDEE, and nutrition calculations.", "Personal Information" },
                    { 7, "[\"Sedentary\",\"Lightly Active\",\"Moderately Active\",\"Very Active\"]", "This multiplier is used for maintenance calorie calculations.", "What is your daily activity level?" },
                    { 8, "[\"Omnivore\",\"Vegetarian\",\"Vegan\",\"Healthy Balanced Diet\"]", "Meal suggestions will follow this preference when possible.", "What is your preferred diet type?" },
                    { 9, "[\"3 meals\",\"4 meals\",\"5 meals\",\"6 or more meals\"]", "Calories will be split across this number of meals.", "How many meals would you like to eat per day?" },
                    { 10, "[\"Calorie Deficit (Fat Loss)\",\"Calorie Surplus (Muscle Gain)\",\"Balanced Nutrition\",\"Performance and Energy\"]", "This determines the macronutrient distribution.", "What is your nutrition priority?" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DropColumn(
                name: "Bmi",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "Bmr",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "Tdee",
                table: "UserProfiles");
        }
    }
}
