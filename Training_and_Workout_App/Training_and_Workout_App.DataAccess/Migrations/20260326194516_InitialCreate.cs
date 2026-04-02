using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training_and_Workout_App.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    MuscleGroup = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GifUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Subtitle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Options = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MealPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Meals = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealPlans_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionnaireEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Skipped = table.Column<bool>(type: "bit", nullable: false),
                    Answers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionnaireEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionnaireEntries_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionnaireEntries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutPlans_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DayPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WorkoutPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DayPlans_WorkoutPlans_WorkoutPlanId",
                        column: x => x.WorkoutPlanId,
                        principalTable: "WorkoutPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutTrackingStates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkoutPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutTrackingStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutTrackingStates_WorkoutPlans_WorkoutPlanId",
                        column: x => x.WorkoutPlanId,
                        principalTable: "WorkoutPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DayPlanExercises",
                columns: table => new
                {
                    DayPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayPlanExercises", x => new { x.DayPlanId, x.ExerciseId });
                    table.ForeignKey(
                        name: "FK_DayPlanExercises_DayPlans_DayPlanId",
                        column: x => x.DayPlanId,
                        principalTable: "DayPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DayPlanExercises_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PauseTimes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Minutes = table.Column<int>(type: "int", nullable: false),
                    Seconds = table.Column<int>(type: "int", nullable: false),
                    WorkoutTrackingStateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PauseTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PauseTimes_WorkoutTrackingStates_WorkoutTrackingStateId",
                        column: x => x.WorkoutTrackingStateId,
                        principalTable: "WorkoutTrackingStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutSets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    Reps = table.Column<int>(type: "int", nullable: false),
                    WorkoutTrackingStateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutSets_WorkoutTrackingStates_WorkoutTrackingStateId",
                        column: x => x.WorkoutTrackingStateId,
                        principalTable: "WorkoutTrackingStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DayPlanExercises_ExerciseId",
                table: "DayPlanExercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_DayPlans_WorkoutPlanId",
                table: "DayPlans",
                column: "WorkoutPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_MealPlans_UserId",
                table: "MealPlans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PauseTimes_WorkoutTrackingStateId",
                table: "PauseTimes",
                column: "WorkoutTrackingStateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireEntries_QuestionId",
                table: "QuestionnaireEntries",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireEntries_UserId",
                table: "QuestionnaireEntries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPlans_UserId",
                table: "WorkoutPlans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSets_WorkoutTrackingStateId",
                table: "WorkoutSets",
                column: "WorkoutTrackingStateId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTrackingStates_WorkoutPlanId",
                table: "WorkoutTrackingStates",
                column: "WorkoutPlanId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DayPlanExercises");

            migrationBuilder.DropTable(
                name: "MealPlans");

            migrationBuilder.DropTable(
                name: "PauseTimes");

            migrationBuilder.DropTable(
                name: "QuestionnaireEntries");

            migrationBuilder.DropTable(
                name: "WorkoutSets");

            migrationBuilder.DropTable(
                name: "DayPlans");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "WorkoutTrackingStates");

            migrationBuilder.DropTable(
                name: "WorkoutPlans");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
