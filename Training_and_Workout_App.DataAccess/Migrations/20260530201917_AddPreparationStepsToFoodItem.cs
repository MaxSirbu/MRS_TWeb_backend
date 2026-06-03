using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training_and_Workout_App.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddPreparationStepsToFoodItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreparationSteps",
                table: "FoodItems",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreparationSteps",
                table: "FoodItems");
        }
    }
}
