using CommandsService.Model;
using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data
{
	public class CommandDbContext : DbContext
	{
		public CommandDbContext(DbContextOptions<CommandDbContext> opt) : base(opt)
		{

		}
		public DbSet<Platform> Platforms { get; set; }

		public DbSet<Command> Commands { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder
				.Entity<Platform>()
				.HasMany(p => p.Commands)
				.WithOne(p => p.Platform!)
				.HasForeignKey(p => p.PlatformId);

			modelBuilder
				.Entity<Command>()
				.HasOne(p => p.Platform)
				.WithMany(p => p.Commands)
				.HasForeignKey(p => p.PlatformId);


		}

	}
}
