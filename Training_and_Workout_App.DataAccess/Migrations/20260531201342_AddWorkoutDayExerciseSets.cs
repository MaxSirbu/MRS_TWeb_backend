using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training_and_Workout_App.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkoutDayExerciseSets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Recommended",
                table: "Exercises");

            migrationBuilder.AlterColumn<int>(
                name: "WorkoutTrackingStateId",
                table: "WorkoutSets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DayPlanId",
                table: "WorkoutSets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExerciseId",
                table: "WorkoutSets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "WorkoutSets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSets_DayPlanId_ExerciseId",
                table: "WorkoutSets",
                columns: new[] { "DayPlanId", "ExerciseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutSets_DayPlanExercises_DayPlanId_ExerciseId",
                table: "WorkoutSets",
                columns: new[] { "DayPlanId", "ExerciseId" },
                principalTable: "DayPlanExercises",
                principalColumns: new[] { "DayPlanId", "ExerciseId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutSets_DayPlanExercises_DayPlanId_ExerciseId",
                table: "WorkoutSets");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutSets_DayPlanId_ExerciseId",
                table: "WorkoutSets");

            migrationBuilder.DropColumn(
                name: "DayPlanId",
                table: "WorkoutSets");

            migrationBuilder.DropColumn(
                name: "ExerciseId",
                table: "WorkoutSets");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "WorkoutSets");

            migrationBuilder.AlterColumn<int>(
                name: "WorkoutTrackingStateId",
                table: "WorkoutSets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Recommended",
                table: "Exercises",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
