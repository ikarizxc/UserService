using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UserService.Domain.Models;
using UserService.Domain.Options;
using UserService.Infrastructure.Database.Configurations;

namespace UserService.Infrastructure.Database
{
	public class UserServiceDbContext : DbContext
	{
		private readonly IOptions<AuthorizationOptions> _authOptions;

		public UserServiceDbContext(
			DbContextOptions<UserServiceDbContext> options,
			IOptions<AuthorizationOptions> authOptions)
			: base(options)
		{
			_authOptions = authOptions;
			Database.EnsureCreated();
		}

		public DbSet<User> Users { get; set; }
		public DbSet<RefreshToken> RefreshTokens { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<Permission> Permissions { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
			modelBuilder.ApplyConfiguration(new UserConfiguration());
			modelBuilder.ApplyConfiguration(new RoleConfiguration());
			modelBuilder.ApplyConfiguration(new PermissionConfiguration());
			modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
			modelBuilder.ApplyConfiguration(new RolePermissionConfigiration(_authOptions.Value));

			base.OnModelCreating(modelBuilder);
		}
	}
}
