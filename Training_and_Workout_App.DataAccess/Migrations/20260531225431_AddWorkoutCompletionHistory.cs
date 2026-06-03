using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training_and_Workout_App.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkoutCompletionHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkoutCompletions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    WorkoutPlanId = table.Column<int>(type: "int", nullable: false),
                    WorkoutDayId = table.Column<int>(type: "int", nullable: false),
                    ScheduledDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CycleNumber = table.Column<int>(type: "int", nullable: false),
                    DayNumber = table.Column<int>(type: "int", nullable: false),
                    TotalVolume = table.Column<double>(type: "float", nullable: false),
                    TotalSets = table.Column<int>(type: "int", nullable: false),
                    TotalExercises = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutCompletions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutCompletions_DayPlans_WorkoutDayId",
                        column: x => x.WorkoutDayId,
                        principalTable: "DayPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkoutCompletions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkoutCompletions_WorkoutPlans_WorkoutPlanId",
                        column: x => x.WorkoutPlanId,
                        principalTable: "WorkoutPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutCompletionExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkoutCompletionId = table.Column<int>(type: "int", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    ExerciseNameSnapshot = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    MuscleGroupSnapshot = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutCompletionExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutCompletionExercises_WorkoutCompletions_WorkoutCompletionId",
                        column: x => x.WorkoutCompletionId,
                        principalTable: "WorkoutCompletions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutCompletionSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkoutCompletionExerciseId = table.Column<int>(type: "int", nullable: false),
                    SetNumber = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    Reps = table.Column<int>(type: "int", nullable: false),
                    Volume = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutCompletionSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutCompletionSets_WorkoutCompletionExercises_WorkoutCompletionExerciseId",
                        column: x => x.WorkoutCompletionExerciseId,
                        principalTable: "WorkoutCompletionExercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutCompletionExercises_WorkoutCompletionId",
                table: "WorkoutCompletionExercises",
                column: "WorkoutCompletionId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutCompletions_UserId_WorkoutPlanId_ScheduledDate",
                table: "WorkoutCompletions",
                columns: new[] { "UserId", "WorkoutPlanId", "ScheduledDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutCompletions_WorkoutDayId",
                table: "WorkoutCompletions",
                column: "WorkoutDayId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutCompletions_WorkoutPlanId",
                table: "WorkoutCompletions",
                column: "WorkoutPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutCompletionSets_WorkoutCompletionExerciseId",
                table: "WorkoutCompletionSets",
                column: "WorkoutCompletionExerciseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkoutCompletionSets");

            migrationBuilder.DropTable(
                name: "WorkoutCompletionExercises");

            migrationBuilder.DropTable(
                name: "WorkoutCompletions");
        }
    }
}
