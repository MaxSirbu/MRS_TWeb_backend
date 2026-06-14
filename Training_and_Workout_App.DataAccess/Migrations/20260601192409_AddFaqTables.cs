using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Training_and_Workout_App.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddFaqTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FaqCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaqCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FaqQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FaqCategoryId = table.Column<int>(type: "int", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaqQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaqQuestions_FaqCategories_FaqCategoryId",
                        column: x => x.FaqCategoryId,
                        principalTable: "FaqCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "FaqCategories",
                columns: new[] { "Id", "Icon", "Title" },
                values: new object[] { 1, "sparkles", "Getting Started" });

            migrationBuilder.InsertData(
                table: "FaqCategories",
                columns: new[] { "Id", "Icon", "Order", "Title" },
                values: new object[,]
                {
                    { 2, "calendar", 1, "Workout Planning" },
                    { 3, "search", 2, "Exercise Library" },
                    { 4, "user", 3, "Profile & Progress" },
                    { 5, "settings", 4, "Technical & Account" }
                });

            migrationBuilder.InsertData(
                table: "FaqQuestions",
                columns: new[] { "Id", "Answer", "FaqCategoryId", "Icon", "Question" },
                values: new object[] { 1, "Navigate to My Plans from the sidebar, click the + button to create a new plan. Choose between Workout or Alimentation, give it a name, and start adding days and exercises.", 1, "dumbbell", "How do I create my first workout plan?" });

            migrationBuilder.InsertData(
                table: "FaqQuestions",
                columns: new[] { "Id", "Answer", "FaqCategoryId", "Icon", "Order", "Question" },
                values: new object[] { 2, "The questionnaire helps us personalize your experience. It collects your fitness goals, activity level, limitations, and preferred workout schedule.", 1, "help", 1, "What is the onboarding questionnaire for?" });

            migrationBuilder.InsertData(
                table: "FaqQuestions",
                columns: new[] { "Id", "Answer", "FaqCategoryId", "Icon", "Question" },
                values: new object[] { 3, "Inside a workout plan, click Add Exercise in the Activities panel. Browse or search the ExerciseData Library, then tap an exercise to add it to the current day.", 2, "dumbbell", "How do I add exercises to my workout?" });

            migrationBuilder.InsertData(
                table: "FaqQuestions",
                columns: new[] { "Id", "Answer", "FaqCategoryId", "Icon", "Order", "Question" },
                values: new object[] { 4, "Each exercise in your plan has an Add set button. Click it to add sets with reps and weight fields that you can fill in as you train.", 2, "calendar", 1, "How do I track sets, reps, and weight?" });

            migrationBuilder.InsertData(
                table: "FaqQuestions",
                columns: new[] { "Id", "Answer", "FaqCategoryId", "Icon", "Question" },
                values: new object[] { 5, "The library contains exercises for major muscle groups including chest, back, legs, arms, core, and cardio.", 3, "search", "How many exercises are in the library?" });

            migrationBuilder.InsertData(
                table: "FaqQuestions",
                columns: new[] { "Id", "Answer", "FaqCategoryId", "Icon", "Order", "Question" },
                values: new object[] { 6, "Yes. Each exercise card includes instructions, and the detail view shows a full breakdown of how to perform it.", 3, "help", 1, "Do exercises include instructions?" });

            migrationBuilder.InsertData(
                table: "FaqQuestions",
                columns: new[] { "Id", "Answer", "FaqCategoryId", "Icon", "Question" },
                values: new object[] { 7, "Go to the Profile page from the sidebar. You can view account details, fitness statistics, and active plan information there.", 4, "user", "How do I update my profile information?" });

            migrationBuilder.InsertData(
                table: "FaqQuestions",
                columns: new[] { "Id", "Answer", "FaqCategoryId", "Icon", "Order", "Question" },
                values: new object[] { 8, "FitLife tracks your workout plans, active plans, exercises, streak, and profile progress.", 4, "sparkles", 1, "What stats does FitLife track?" });

            migrationBuilder.InsertData(
                table: "FaqQuestions",
                columns: new[] { "Id", "Answer", "FaqCategoryId", "Icon", "Question" },
                values: new object[] { 9, "Yes. Your plans, profile, questionnaire answers, favorites, and completions are saved in your account database.", 5, "settings", "Is my workout data saved?" });

            migrationBuilder.InsertData(
                table: "FaqQuestions",
                columns: new[] { "Id", "Answer", "FaqCategoryId", "Icon", "Order", "Question" },
                values: new object[] { 10, "Click the Logout button at the bottom of the sidebar. You will be redirected to the login page and your session will end.", 5, "message", 1, "How do I sign out?" });

            migrationBuilder.CreateIndex(
                name: "IX_FaqQuestions_FaqCategoryId",
                table: "FaqQuestions",
                column: "FaqCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FaqQuestions");

            migrationBuilder.DropTable(
                name: "FaqCategories");
        }
    }
}
