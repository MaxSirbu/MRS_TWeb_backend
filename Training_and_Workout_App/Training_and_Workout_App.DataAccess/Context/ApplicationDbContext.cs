using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;
using Training_and_Workout_App.Domain.Entities;

namespace Training_and_Workout_App.DataAccess.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<WorkoutPlan> WorkoutPlans => Set<WorkoutPlan>();
    public DbSet<DayPlan> DayPlans => Set<DayPlan>();
    public DbSet<DayPlanExercise> DayPlanExercises => Set<DayPlanExercise>();
    public DbSet<WorkoutTrackingState> WorkoutTrackingStates => Set<WorkoutTrackingState>();
    public DbSet<WorkoutSet> WorkoutSets => Set<WorkoutSet>();
    public DbSet<PauseTime> PauseTimes => Set<PauseTime>();
    public DbSet<MealPlan> MealPlans => Set<MealPlan>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<QuestionnaireEntry> QuestionnaireEntries => Set<QuestionnaireEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── User ────────────────────────────────────────────────────────────
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.FullName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Password).IsRequired().HasMaxLength(256);
        });

        // ── Exercise ─────────────────────────────────────────────────────────
        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
            entity.Property(e => e.GifUrl).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Instructions).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.MuscleGroup).HasConversion<string>().HasMaxLength(20);
        });

        // ── WorkoutPlan ──────────────────────────────────────────────────────
        modelBuilder.Entity<WorkoutPlan>(entity =>
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
                  .HasForeignKey<WorkoutTrackingState>(wt => wt.WorkoutPlanId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── DayPlan ──────────────────────────────────────────────────────────
        modelBuilder.Entity<DayPlan>(entity =>
        {
            entity.HasKey(dp => dp.Id);
            entity.Property(dp => dp.Label).IsRequired().HasMaxLength(50);

            // DayPlan -> WorkoutPlan (many-to-one)
            entity.HasOne(dp => dp.WorkoutPlan)
                  .WithMany(wp => wp.Days)
                  .HasForeignKey(dp => dp.WorkoutPlanId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── DayPlanExercise (junction) ───────────────────────────────────────
        modelBuilder.Entity<DayPlanExercise>(entity =>
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

            entity.Property(dpe => dpe.Order).HasDefaultValue(0);
        });

        // ── WorkoutTrackingState ─────────────────────────────────────────────
        modelBuilder.Entity<WorkoutTrackingState>(entity =>
        {
            entity.HasKey(wt => wt.Id);

            // WorkoutTrackingState -> PauseTime (one-to-one)
            entity.HasOne(wt => wt.PauseTime)
                  .WithOne(pt => pt.WorkoutTrackingState)
                  .HasForeignKey<PauseTime>(pt => pt.WorkoutTrackingStateId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── WorkoutSet ───────────────────────────────────────────────────────
        modelBuilder.Entity<WorkoutSet>(entity =>
        {
            entity.HasKey(ws => ws.Id);

            entity.HasOne(ws => ws.WorkoutTrackingState)
                  .WithMany(wt => wt.Sets)
                  .HasForeignKey(ws => ws.WorkoutTrackingStateId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── PauseTime ────────────────────────────────────────────────────────
        modelBuilder.Entity<PauseTime>(entity =>
        {
            entity.HasKey(pt => pt.Id);
        });

        // ── MealPlan ─────────────────────────────────────────────────────────
        modelBuilder.Entity<MealPlan>(entity =>
        {
            entity.HasKey(mp => mp.Id);
            entity.Property(mp => mp.Name).IsRequired().HasMaxLength(100);
            entity.Property(mp => mp.UpdatedAt).IsRequired();

            entity.HasOne(mp => mp.User)
                  .WithMany(u => u.MealPlans)
                  .HasForeignKey(mp => mp.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── Question ─────────────────────────────────────────────────────────
        modelBuilder.Entity<Question>(entity =>
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
        modelBuilder.Entity<QuestionnaireEntry>(entity =>
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
    }
}

