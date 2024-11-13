using UserService.API;

CreateHostBuilder(args).Build().Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
	Host.CreateDefaultBuilder(args)
		.ConfigureWebHostDefaults(
		webBuilder =>
		{
			webBuilder.UseStartup<Startup>();
			webBuilder.UseUrls("http://*:36801");
		});