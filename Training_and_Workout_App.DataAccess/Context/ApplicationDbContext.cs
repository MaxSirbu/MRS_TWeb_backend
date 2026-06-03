using Training_and_Workout_App.Domain.Entities.Exercise;
using Training_and_Workout_App.Domain.Entities.FAQ;
using Training_and_Workout_App.Domain.Entities.FoodItem;
using Training_and_Workout_App.Domain.Entities.MealDayEntry;
using Training_and_Workout_App.Domain.Entities.Plans;
using Training_and_Workout_App.Domain.Entities.PlanState;
using Training_and_Workout_App.Domain.Entities.Question;
using Training_and_Workout_App.Domain.Entities.QuestionnaireEntry;
using Training_and_Workout_App.Domain.Entities.User;
using Training_and_Workout_App.Domain.Entities.WorkoutHistory;
using Training_and_Workout_App.Domain.Entities.WorkoutTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace Training_and_Workout_App.DataAccess.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<UserData> Users => Set<UserData>();
    public DbSet<ExerciseData> Exercises => Set<ExerciseData>();
    public DbSet<WorkoutPlanData> WorkoutPlans => Set<WorkoutPlanData>();
    public DbSet<DayPlanData> DayPlans => Set<DayPlanData>();
    public DbSet<DayPlanExerciseData> DayPlanExercises => Set<DayPlanExerciseData>();
    public DbSet<WorkoutTrackingStateData> WorkoutTrackingStates => Set<WorkoutTrackingStateData>();
    public DbSet<WorkoutSetData> WorkoutSets => Set<WorkoutSetData>();
    public DbSet<WorkoutCompletionData> WorkoutCompletions => Set<WorkoutCompletionData>();
    public DbSet<WorkoutCompletionExerciseData> WorkoutCompletionExercises => Set<WorkoutCompletionExerciseData>();
    public DbSet<WorkoutCompletionSetData> WorkoutCompletionSets => Set<WorkoutCompletionSetData>();
    public DbSet<PauseTimeData> PauseTimes => Set<PauseTimeData>();
    public DbSet<MealPlanData> MealPlans => Set<MealPlanData>();
    public DbSet<MealPlanDayData> MealPlanDays => Set<MealPlanDayData>();
    public DbSet<MealCategoryData> MealCategories => Set<MealCategoryData>();
    public DbSet<MealCategoryFoodItemData> MealCategoryFoodItems => Set<MealCategoryFoodItemData>();
    public DbSet<QuestionData> Questions => Set<QuestionData>();
    public DbSet<FaqCategoryData> FaqCategories => Set<FaqCategoryData>();
    public DbSet<FaqQuestionData> FaqQuestions => Set<FaqQuestionData>();
    public DbSet<QuestionnaireEntryData> QuestionnaireEntries => Set<QuestionnaireEntryData>();
    public DbSet<FoodItemData> FoodItems => Set<FoodItemData>();
    public DbSet<UserProfileData> UserProfiles => Set<UserProfileData>();
    public DbSet<PlanActivationData> PlanActivations => Set<PlanActivationData>();
    public DbSet<PlanCompletionData> PlanCompletions => Set<PlanCompletionData>();
    public DbSet<PlanCustomizationData> PlanCustomizations => Set<PlanCustomizationData>();
    public DbSet<UserPlanFavoriteData> UserPlanFavorites => Set<UserPlanFavoriteData>();
    public DbSet<MealDayEntryData> MealDayEntries => Set<MealDayEntryData>();
    public DbSet<MealDayEntryFoodItemData> MealDayEntryFoodItems => Set<MealDayEntryFoodItemData>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── User ────────────────────────────────────────────────────────────
        modelBuilder.Entity<UserData>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.FullName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Password).IsRequired().HasMaxLength(256);
            entity.Property(u => u.Username).IsRequired().HasMaxLength(256);
            entity.HasIndex(u => u.Username).IsUnique();
            entity.Property(u => u.Role).HasConversion<string>().HasMaxLength(20);
        });

        // ── Exercise ─────────────────────────────────────────────────────────
        modelBuilder.Entity<ExerciseData>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
            entity.Property(e => e.GifUrl).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Instructions).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.MuscleGroup).HasConversion<string>().HasMaxLength(20);
        });

        // ── WorkoutPlan ──────────────────────────────────────────────────────
        modelBuilder.Entity<WorkoutPlanData>(entity =>
        {
            entity.HasKey(wp => wp.Id);
            entity.Property(wp => wp.Name).IsRequired().HasMaxLength(100);
            entity.Property(wp => wp.CreatedAt).IsRequired();
            entity.Property(wp => wp.UpdatedAt).IsRequired();

            // WorkoutPlan -> User (many-to-one)
            entity.HasOne(wp => wp.User)
                  .WithMany(u => u.WorkoutPlans)
                  .HasForeignKey(wp => wp.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            // WorkoutPlan -> WorkoutTrackingState (one-to-one)
            entity.HasOne(wp => wp.WorkoutTracking)
                  .WithOne(wt => wt.WorkoutPlan)
                  .HasForeignKey<WorkoutTrackingStateData>(wt => wt.WorkoutPlanId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── DayPlan ──────────────────────────────────────────────────────────
        modelBuilder.Entity<DayPlanData>(entity =>
        {
            entity.HasKey(dp => dp.Id);
            entity.Property(dp => dp.Label).IsRequired().HasMaxLength(50);
            entity.Property(dp => dp.DayNumber).IsRequired();

            // DayPlan -> WorkoutPlan (many-to-one)
            entity.HasOne(dp => dp.WorkoutPlan)
                  .WithMany(wp => wp.Days)
                  .HasForeignKey(dp => dp.WorkoutPlanId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── DayPlanExercise (junction) ───────────────────────────────────────
        modelBuilder.Entity<DayPlanExerciseData>(entity =>
        {
            entity.HasKey(dpe => new { dpe.DayPlanId, dpe.ExerciseId });

            entity.HasOne(dpe => dpe.DayPlan)
                  .WithMany(dp => dp.DayPlanExercises)
                  .HasForeignKey(dpe => dpe.DayPlanId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(dpe => dpe.Exercise)
                  .WithMany(e => e.DayPlanExercises)
                  .HasForeignKey(dpe => dpe.ExerciseId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(dpe => dpe.PauseTime)
                  .WithOne(pt => pt.DayPlanExercise)
                  .HasForeignKey<PauseTimeData>(pt => new { pt.DayPlanId, pt.ExerciseId })
                  .OnDelete(DeleteBehavior.NoAction);

            entity.Property(dpe => dpe.Order).HasDefaultValue(0);
        });

        // ── WorkoutTrackingState ─────────────────────────────────────────────
        modelBuilder.Entity<WorkoutTrackingStateData>(entity =>
        {
            entity.HasKey(wt => wt.Id);

            // WorkoutTrackingState -> PauseTime (one-to-one)
            entity.HasOne(wt => wt.PauseTime)
                  .WithOne(pt => pt.WorkoutTrackingState)
                  .HasForeignKey<PauseTimeData>(pt => pt.WorkoutTrackingStateId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── WorkoutSet ───────────────────────────────────────────────────────
        modelBuilder.Entity<WorkoutSetData>(entity =>
        {
            entity.HasKey(ws => ws.Id);

            entity.HasOne(ws => ws.WorkoutTrackingState)
                  .WithMany(wt => wt.Sets)
                  .HasForeignKey(ws => ws.WorkoutTrackingStateId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ws => ws.DayPlanExercise)
                  .WithMany(dpe => dpe.Sets)
                  .HasForeignKey(ws => new { ws.DayPlanId, ws.ExerciseId })
                  .OnDelete(DeleteBehavior.Cascade);

            entity.Property(ws => ws.Order).HasDefaultValue(0);
        });

        // ── PauseTime ────────────────────────────────────────────────────────
        modelBuilder.Entity<PauseTimeData>(entity =>
        {
            entity.HasKey(pt => pt.Id);
            entity.HasIndex(pt => new { pt.DayPlanId, pt.ExerciseId })
                  .IsUnique()
                  .HasFilter("[DayPlanId] IS NOT NULL AND [ExerciseId] IS NOT NULL");
        });

        // ── MealPlan ─────────────────────────────────────────────────────────
        modelBuilder.Entity<MealPlanData>(entity =>
        {
            entity.HasKey(mp => mp.Id);
            entity.Property(mp => mp.Name).IsRequired().HasMaxLength(100);
            entity.Property(mp => mp.Description).HasMaxLength(500);
            entity.Property(mp => mp.ImageUrl).HasMaxLength(500);
            entity.Property(mp => mp.UpdatedAt).IsRequired();

            entity.HasOne(mp => mp.User)
                  .WithMany(u => u.MealPlans)
                  .HasForeignKey(mp => mp.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MealPlanDayData>(entity =>
        {
            entity.HasKey(day => day.Id);
            entity.Property(day => day.Label).IsRequired().HasMaxLength(50);

            entity.HasOne(day => day.MealPlan)
                  .WithMany()
                  .HasForeignKey(day => day.MealPlanId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MealCategoryData>(entity =>
        {
            entity.HasKey(category => category.Id);
            entity.Property(category => category.Slot).HasConversion<string>().HasMaxLength(20);
            entity.Property(category => category.Order).HasDefaultValue(0);

            entity.HasOne(category => category.MealPlanDay)
                  .WithMany(day => day.Categories)
                  .HasForeignKey(category => category.MealPlanDayId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MealCategoryFoodItemData>(entity =>
        {
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Order).HasDefaultValue(0);
            entity.Property(item => item.QuantityGrams).HasDefaultValue(100.0);

            entity.HasOne(item => item.MealCategory)
                  .WithMany(category => category.Items)
                  .HasForeignKey(item => item.MealCategoryId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(item => item.FoodItem)
                  .WithMany()
                  .HasForeignKey(item => item.FoodItemId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Question ─────────────────────────────────────────────────────────
        modelBuilder.Entity<QuestionData>(entity =>
        {
            entity.HasKey(q => q.Id);
            entity.Property(q => q.Title).IsRequired().HasMaxLength(300);
            entity.Property(q => q.Subtitle).IsRequired().HasMaxLength(500);

            // Store List<string> as JSON column
            entity.Property(q => q.Options)
                  .HasConversion(
                      v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                      v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>(),
                      new ValueComparer<List<string>>(
                          (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                          c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                          c => c.ToList()))
                  .HasColumnType("nvarchar(max)");
        });

        // ── QuestionnaireEntry ───────────────────────────────────────────────
        modelBuilder.Entity<QuestionnaireEntryData>(entity =>
        {
            entity.HasKey(qe => qe.Id);
            entity.Property(qe => qe.CompletedAt).IsRequired();

            // Store Dictionary<string,string>? as JSON column
            entity.Property(qe => qe.Answers)
                  .HasConversion(
                      v => v == null ? null : JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                      v => v == null ? null : JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions?)null),
                      new ValueComparer<Dictionary<string, string>?>(
                          (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                          c => c == null ? 0 : c.Aggregate(0, (a, kv) => HashCode.Combine(a, kv.Key.GetHashCode(), kv.Value.GetHashCode())),
                          c => c == null ? null : new Dictionary<string, string>(c)))
                  .HasColumnType("nvarchar(max)");

            entity.HasOne(qe => qe.User)
                  .WithMany(u => u.QuestionnaireEntries)
                  .HasForeignKey(qe => qe.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(qe => qe.Question)
                  .WithMany(q => q.QuestionnaireEntries)
                  .HasForeignKey(qe => qe.QuestionId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<FaqCategoryData>(entity =>
        {
            entity.HasKey(category => category.Id);
            entity.Property(category => category.Title).IsRequired().HasMaxLength(120);
            entity.Property(category => category.Icon).IsRequired().HasMaxLength(40);
            entity.Property(category => category.Order).HasDefaultValue(0);
        });

        modelBuilder.Entity<FaqQuestionData>(entity =>
        {
            entity.HasKey(question => question.Id);
            entity.Property(question => question.Question).IsRequired().HasMaxLength(300);
            entity.Property(question => question.Answer).IsRequired().HasMaxLength(2000);
            entity.Property(question => question.Icon).IsRequired().HasMaxLength(40);
            entity.Property(question => question.Order).HasDefaultValue(0);

            entity.HasOne(question => question.Category)
                  .WithMany(category => category.Questions)
                  .HasForeignKey(question => question.FaqCategoryId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── FoodItem ─────────────────────────────────────────────────────────
        modelBuilder.Entity<FoodItemData>(entity =>
        {
            entity.HasKey(f => f.Id);
            entity.Property(f => f.Name).IsRequired().HasMaxLength(150);
            entity.Property(f => f.Category).IsRequired().HasMaxLength(100);
            entity.Property(f => f.ImageUrl).HasMaxLength(500);
            entity.Property(f => f.Description).HasMaxLength(2000);
            entity.Property(f => f.ItemType).HasConversion<string>().HasMaxLength(20);
        });

        // ── UserProfile ──────────────────────────────────────────────────────
        modelBuilder.Entity<UserProfileData>(entity =>
        {
            entity.HasKey(up => up.Id);
            entity.Property(up => up.AvatarUrl).HasMaxLength(500);

            entity.HasOne(up => up.User)
                  .WithOne(u => u.UserProfile)
                  .HasForeignKey<UserProfileData>(up => up.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── PlanActivation ───────────────────────────────────────────────────
        modelBuilder.Entity<PlanActivationData>(entity =>
        {
            entity.HasKey(pa => pa.Id);
            entity.Property(pa => pa.PlanIdentifier).IsRequired().HasMaxLength(100);
            entity.Property(pa => pa.PlanType).HasConversion<string>().HasMaxLength(20);

            entity.HasOne(pa => pa.User)
                  .WithMany(u => u.PlanActivations)
                  .HasForeignKey(pa => pa.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── PlanCompletion ───────────────────────────────────────────────────
        modelBuilder.Entity<PlanCompletionData>(entity =>
        {
            entity.HasKey(pc => pc.Id);
            entity.Property(pc => pc.DayToken).IsRequired().HasMaxLength(200);
            entity.Property(pc => pc.PlanType).HasConversion<string>().HasMaxLength(20);

            entity.HasOne(pc => pc.User)
                  .WithMany(u => u.PlanCompletions)
                  .HasForeignKey(pc => pc.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── PlanCustomization ────────────────────────────────────────────────
        modelBuilder.Entity<PlanCustomizationData>(entity =>
        {
            entity.HasKey(pc => pc.Id);
            entity.Property(pc => pc.PlanIdentifier).IsRequired().HasMaxLength(100);
            entity.Property(pc => pc.ColorId).HasMaxLength(50);
            entity.Property(pc => pc.ImageUrl).HasMaxLength(500);
            entity.Property(pc => pc.PlanType).HasConversion<string>().HasMaxLength(20);

            entity.HasOne(pc => pc.User)
                  .WithMany(u => u.PlanCustomizations)
                  .HasForeignKey(pc => pc.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── UserPlanFavorite ─────────────────────────────────────────────────
        modelBuilder.Entity<UserPlanFavoriteData>(entity =>
        {
            entity.HasKey(f => f.Id);
            entity.Property(f => f.PlanIdentifier).IsRequired().HasMaxLength(100);
            entity.Property(f => f.PlanType).HasConversion<string>().HasMaxLength(20);

            entity.HasOne(f => f.User)
                  .WithMany(u => u.PlanFavorites)
                  .HasForeignKey(f => f.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── MealDayEntry ─────────────────────────────────────────────────────
        modelBuilder.Entity<MealDayEntryData>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.MealPlanIdentifier).IsRequired().HasMaxLength(100);
            entity.Property(m => m.DayId).IsRequired().HasMaxLength(20);
            entity.Property(m => m.MealSlot).HasConversion<string>().HasMaxLength(20);

            entity.HasOne(m => m.User)
                  .WithMany(u => u.MealDayEntries)
                  .HasForeignKey(m => m.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── MealDayEntryFoodItem (junction) ──────────────────────────────────
        modelBuilder.Entity<MealDayEntryFoodItemData>(entity =>
        {
            entity.HasKey(mf => new { mf.MealDayEntryId, mf.FoodItemId });

            entity.HasOne(mf => mf.MealDayEntry)
                  .WithMany(m => m.MealDayEntryFoodItems)
                  .HasForeignKey(mf => mf.MealDayEntryId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(mf => mf.FoodItem)
                  .WithMany()
                  .HasForeignKey(mf => mf.FoodItemId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(mf => mf.Order).HasDefaultValue(0);
        });
    }
}

