using UserService;
using UserService.Infrastructure.Database;

CreateHostBuilder(args).Build().Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
	Host.CreateDefaultBuilder(args)
		.ConfigureAppConfiguration((hostingContext, config) =>
		{
			config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
		})
		.ConfigureWebHostDefaults(
		webBuilder =>
		{
			webBuilder.UseStartup<Startup>();
		});