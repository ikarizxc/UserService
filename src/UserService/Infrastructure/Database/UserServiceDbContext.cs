using Microsoft.EntityFrameworkCore;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Database
{
	public class UserServiceDbContext : DbContext
	{
		public UserServiceDbContext(DbContextOptions<UserServiceDbContext> options)
			: base(options)
		{
			Database.EnsureCreated();
			Database.MigrateAsync();
		}

		public DbSet<User> Users { get; set; }
		public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
