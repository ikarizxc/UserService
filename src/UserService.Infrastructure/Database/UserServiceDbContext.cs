using Microsoft.EntityFrameworkCore;
using UserService.Domain.Models;
using UserService.Infrastructure.Database.Configurations;

namespace UserService.Infrastructure.Database
{
	public class UserServiceDbContext : DbContext
	{
		public UserServiceDbContext(DbContextOptions<UserServiceDbContext> options)
			: base(options)
		{
			Database.EnsureCreated();
		}

		public DbSet<User> Users { get; set; }
		public DbSet<RefreshToken> RefreshTokens { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new RefreshTokensConfiguration());
			modelBuilder.ApplyConfiguration(new UsersConfiguration());
		}
	}
}
