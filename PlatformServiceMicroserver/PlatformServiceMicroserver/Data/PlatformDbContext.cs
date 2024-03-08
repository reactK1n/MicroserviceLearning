using Microsoft.EntityFrameworkCore;
using PlatformServiceMicroserver.Models;

namespace PlatformServiceMicroserver.Data
{
	public class PlatformDbContext : DbContext
	{
		public PlatformDbContext(DbContextOptions<PlatformDbContext> opt) : base (opt) 
		{ 
		
		}

		public DbSet<Platform> Platforms { get; set; }
	}
}
