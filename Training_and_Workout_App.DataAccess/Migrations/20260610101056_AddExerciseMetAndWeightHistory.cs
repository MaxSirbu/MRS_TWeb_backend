using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training_and_Workout_App.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddExerciseMetAndWeightHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MetValue",
                table: "Exercises",
                type: "float",
                nullable: false,
                defaultValue: 5.0);

            migrationBuilder.CreateTable(
                name: "UserWeightHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWeightHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWeightHistory_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserWeightHistory_UserId_RecordedAt",
                table: "UserWeightHistory",
                columns: new[] { "UserId", "RecordedAt" });

            migrationBuilder.Sql(
                """
                UPDATE [Exercises]
                SET [MetValue] =
                    CASE
                        WHEN [MuscleGroup] = 'Cardio' THEN 7.3
                        WHEN LOWER([Name]) LIKE '%burpee%'
                          OR LOWER([Name]) LIKE '%jump%'
                          OR LOWER([Name]) LIKE '%mountain climber%' THEN 7.5
                        WHEN LOWER([Name]) LIKE '%plank%'
                          OR LOWER([Name]) LIKE '%crunch%' THEN 2.8
                        WHEN LOWER([Name]) LIKE '%push-up%'
                          OR LOWER([Name]) LIKE '%push up%'
                          OR LOWER([Name]) LIKE '%pull-up%'
                          OR LOWER([Name]) LIKE '%pull up%'
                          OR LOWER([Name]) LIKE '%lunge%' THEN 3.8
                        ELSE 5.0
                    END;

                INSERT INTO [UserWeightHistory] ([Weight], [RecordedAt], [UserId])
                SELECT [Weight], SYSUTCDATETIME(), [UserId]
                FROM [UserProfiles]
                WHERE [Weight] > 0;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserWeightHistory");

            migrationBuilder.DropColumn(
                name: "MetValue",
                table: "Exercises");
        }
    }
}
