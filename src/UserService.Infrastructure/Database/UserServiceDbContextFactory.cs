using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using UserService.Domain.Options;
using UserService.Infrastructure.Database;

public class UserServiceDbContextFactory : IDesignTimeDbContextFactory<UserServiceDbContext>
{
	public UserServiceDbContext CreateDbContext(string[] args)
	{
		var configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json")
			.Build();

		var optionsBuilder = new DbContextOptionsBuilder<UserServiceDbContext>();
		optionsBuilder.UseNpgsql(configuration.GetConnectionString("Postgres"));

		var authOptions = Options.Create(configuration.GetSection("AuthorizationOptions").Get<AuthorizationOptions>()) as IOptions<AuthorizationOptions>;

		return new UserServiceDbContext(optionsBuilder.Options, authOptions);
	}
}
