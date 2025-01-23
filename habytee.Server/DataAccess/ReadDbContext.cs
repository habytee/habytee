using habytee.Interconnection.Models;
using Microsoft.EntityFrameworkCore;

namespace habytee.Server.DataAccess;

public class ReadDbContext(DbContextOptions<ReadDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Habit> Habits { get; set; } = null!;
	public DbSet<HabitCheckedEvent> HabitCheckedEvents { get; set; } = null!;

    public override int SaveChanges()
    {
        throw new InvalidOperationException("Tried to save read only context");
    }

    /*public override Task<int> SaveChangesAsync()
    {
        throw new InvalidOperationException("Tried to save read only context");
    }*/

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Habits)
            .WithOne(h => h.User)
            .HasForeignKey(h => h.UserId)
            .IsRequired();

		modelBuilder.Entity<Habit>()
			.HasMany(h => h.HabitCheckedEvents)
			.WithOne(h => h.Habit)
			.HasForeignKey(h => h.HabitId)
			.IsRequired();
	}
}
