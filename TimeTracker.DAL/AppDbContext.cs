using Microsoft.EntityFrameworkCore;
using TimeTracker.DAL.Entities;

namespace TimeTracker.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<TrackedApp> TrackedApps { get; set; } = null!;
        public DbSet<ActivitySession> ActivitySessions { get; set; } = null!;
        public DbSet<Goal> Goals { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ===== Category =====
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(c => c.Name).HasMaxLength(50);
                entity.Property(c => c.Color).HasMaxLength(9);
                entity.HasIndex(c => c.Name).IsUnique();
            });

            // ===== TrackedApp =====
            modelBuilder.Entity<TrackedApp>(entity =>
            {
                entity.HasOne(a => a.Category)
                      .WithMany(c => c.Apps)
                      .HasForeignKey(a => a.CategoryId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.Property(a => a.ProcessName).HasMaxLength(260);
                entity.Property(a => a.DisplayName).HasMaxLength(200);
                entity.HasIndex(a => a.ProcessName).IsUnique();
            });

            // ===== ActivitySession =====
            modelBuilder.Entity<ActivitySession>(entity =>
            {
                entity.Property(s => s.WindowTitle).HasMaxLength(500);
            });
        }
    }
}