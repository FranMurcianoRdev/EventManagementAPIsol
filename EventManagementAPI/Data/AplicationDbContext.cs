using EventManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

public class AplicationDbContext : DbContext
{
	public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options) { }

	public DbSet<Event> Events { get; set; }
	public DbSet<Participant> Participants { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Event>()
			.HasMany(e => e.Participants)
			.WithMany(p => p.Events);

		modelBuilder.Entity<Event>()
			.Property(e => e.StartDate)
			.HasColumnType("date");

		modelBuilder.Entity<Event>()
			.Property(e => e.EndDate)
			.HasColumnType("date")
			.IsRequired(false)
			.HasDefaultValue(null)
			.HasAnnotation("CheckConstraint", "EndDate >= StartDate");
	}
}
