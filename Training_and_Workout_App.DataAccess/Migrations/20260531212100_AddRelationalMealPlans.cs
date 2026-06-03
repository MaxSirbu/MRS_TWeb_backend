using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training_and_Workout_App.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationalMealPlans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MealPlanDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DayNumber = table.Column<int>(type: "int", nullable: false),
                    MealPlanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPlanDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealPlanDays_MealPlans_MealPlanId",
                        column: x => x.MealPlanId,
                        principalTable: "MealPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Slot = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MealPlanDayId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealCategories_MealPlanDays_MealPlanDayId",
                        column: x => x.MealPlanDayId,
                        principalTable: "MealPlanDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealCategoryFoodItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealCategoryId = table.Column<int>(type: "int", nullable: false),
                    FoodItemId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    QuantityGrams = table.Column<double>(type: "float", nullable: false, defaultValue: 100.0),
                    Kcal = table.Column<double>(type: "float", nullable: false),
                    Protein = table.Column<double>(type: "float", nullable: false),
                    Carbs = table.Column<double>(type: "float", nullable: false),
                    Fats = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealCategoryFoodItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealCategoryFoodItems_FoodItems_FoodItemId",
                        column: x => x.FoodItemId,
                        principalTable: "FoodItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MealCategoryFoodItems_MealCategories_MealCategoryId",
                        column: x => x.MealCategoryId,
                        principalTable: "MealCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MealCategories_MealPlanDayId",
                table: "MealCategories",
                column: "MealPlanDayId");

            migrationBuilder.CreateIndex(
                name: "IX_MealCategoryFoodItems_FoodItemId",
                table: "MealCategoryFoodItems",
                column: "FoodItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MealCategoryFoodItems_MealCategoryId",
                table: "MealCategoryFoodItems",
                column: "MealCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MealPlanDays_MealPlanId",
                table: "MealPlanDays",
                column: "MealPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MealCategoryFoodItems");

            migrationBuilder.DropTable(
                name: "MealCategories");

            migrationBuilder.DropTable(
                name: "MealPlanDays");
        }
    }
}
