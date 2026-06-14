using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training_and_Workout_App.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddExercisePauseTimes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = N'IX_PauseTimes_WorkoutTrackingStateId'
                      AND object_id = OBJECT_ID(N'[PauseTimes]')
                )
                BEGIN
                    DROP INDEX [IX_PauseTimes_WorkoutTrackingStateId] ON [PauseTimes];
                END
                """);

            migrationBuilder.Sql("""
                IF COL_LENGTH(N'[PauseTimes]', N'DayPlanId') IS NULL
                BEGIN
                    ALTER TABLE [PauseTimes] ADD [DayPlanId] int NULL;
                END
                """);

            migrationBuilder.Sql("""
                IF COL_LENGTH(N'[PauseTimes]', N'ExerciseId') IS NULL
                BEGIN
                    ALTER TABLE [PauseTimes] ADD [ExerciseId] int NULL;
                END
                """);

            migrationBuilder.Sql("""
                DECLARE @constraintName sysname;
                SELECT @constraintName = [d].[name]
                FROM [sys].[default_constraints] [d]
                INNER JOIN [sys].[columns] [c]
                    ON [d].[parent_column_id] = [c].[column_id]
                    AND [d].[parent_object_id] = [c].[object_id]
                WHERE [d].[parent_object_id] = OBJECT_ID(N'[PauseTimes]')
                    AND [c].[name] = N'WorkoutTrackingStateId';

                IF @constraintName IS NOT NULL
                BEGIN
                    EXEC(N'ALTER TABLE [PauseTimes] DROP CONSTRAINT [' + @constraintName + ']');
                END

                IF EXISTS (
                    SELECT 1
                    FROM sys.columns
                    WHERE object_id = OBJECT_ID(N'[PauseTimes]')
                      AND name = N'WorkoutTrackingStateId'
                      AND is_nullable = 0
                )
                BEGIN
                    ALTER TABLE [PauseTimes] ALTER COLUMN [WorkoutTrackingStateId] int NULL;
                END
                """);

            migrationBuilder.Sql("""
                IF NOT EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = N'IX_PauseTimes_DayPlanId_ExerciseId'
                      AND object_id = OBJECT_ID(N'[PauseTimes]')
                )
                BEGIN
                    CREATE UNIQUE INDEX [IX_PauseTimes_DayPlanId_ExerciseId]
                    ON [PauseTimes] ([DayPlanId], [ExerciseId])
                    WHERE [DayPlanId] IS NOT NULL AND [ExerciseId] IS NOT NULL;
                END
                """);

            migrationBuilder.Sql("""
                IF NOT EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = N'IX_PauseTimes_WorkoutTrackingStateId'
                      AND object_id = OBJECT_ID(N'[PauseTimes]')
                )
                BEGIN
                    CREATE UNIQUE INDEX [IX_PauseTimes_WorkoutTrackingStateId]
                    ON [PauseTimes] ([WorkoutTrackingStateId])
                    WHERE [WorkoutTrackingStateId] IS NOT NULL;
                END
                """);

            migrationBuilder.Sql("""
                IF NOT EXISTS (
                    SELECT 1 FROM sys.foreign_keys
                    WHERE name = N'FK_PauseTimes_DayPlanExercises_DayPlanId_ExerciseId'
                )
                BEGIN
                    ALTER TABLE [PauseTimes]
                    ADD CONSTRAINT [FK_PauseTimes_DayPlanExercises_DayPlanId_ExerciseId]
                    FOREIGN KEY ([DayPlanId], [ExerciseId])
                    REFERENCES [DayPlanExercises] ([DayPlanId], [ExerciseId])
                    ON DELETE NO ACTION;
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF EXISTS (
                    SELECT 1 FROM sys.foreign_keys
                    WHERE name = N'FK_PauseTimes_DayPlanExercises_DayPlanId_ExerciseId'
                )
                BEGIN
                    ALTER TABLE [PauseTimes]
                    DROP CONSTRAINT [FK_PauseTimes_DayPlanExercises_DayPlanId_ExerciseId];
                END
                """);

            migrationBuilder.Sql("""
                IF EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = N'IX_PauseTimes_DayPlanId_ExerciseId'
                      AND object_id = OBJECT_ID(N'[PauseTimes]')
                )
                BEGIN
                    DROP INDEX [IX_PauseTimes_DayPlanId_ExerciseId] ON [PauseTimes];
                END
                """);

            migrationBuilder.Sql("""
                IF EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = N'IX_PauseTimes_WorkoutTrackingStateId'
                      AND object_id = OBJECT_ID(N'[PauseTimes]')
                )
                BEGIN
                    DROP INDEX [IX_PauseTimes_WorkoutTrackingStateId] ON [PauseTimes];
                END
                """);

            migrationBuilder.Sql("""
                IF COL_LENGTH(N'[PauseTimes]', N'DayPlanId') IS NOT NULL
                BEGIN
                    ALTER TABLE [PauseTimes] DROP COLUMN [DayPlanId];
                END
                """);

            migrationBuilder.Sql("""
                IF COL_LENGTH(N'[PauseTimes]', N'ExerciseId') IS NOT NULL
                BEGIN
                    ALTER TABLE [PauseTimes] DROP COLUMN [ExerciseId];
                END
                """);

            migrationBuilder.Sql("""
                IF EXISTS (
                    SELECT 1
                    FROM sys.columns
                    WHERE object_id = OBJECT_ID(N'[PauseTimes]')
                      AND name = N'WorkoutTrackingStateId'
                      AND is_nullable = 1
                )
                BEGIN
                    UPDATE [PauseTimes]
                    SET [WorkoutTrackingStateId] = 0
                    WHERE [WorkoutTrackingStateId] IS NULL;

                    ALTER TABLE [PauseTimes] ALTER COLUMN [WorkoutTrackingStateId] int NOT NULL;
                END
                """);

            migrationBuilder.Sql("""
                IF NOT EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = N'IX_PauseTimes_WorkoutTrackingStateId'
                      AND object_id = OBJECT_ID(N'[PauseTimes]')
                )
                BEGIN
                    CREATE UNIQUE INDEX [IX_PauseTimes_WorkoutTrackingStateId]
                    ON [PauseTimes] ([WorkoutTrackingStateId]);
                END
                """);
        }
    }
}
