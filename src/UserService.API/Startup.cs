﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using UserService.Domain.Interfaces.Repositories;
using UserService.Domain.Interfaces.Services;
using UserService.Domain.Options;
using UserService.Domain.Services;
using UserService.Infrastructure.Database;
using UserService.Infrastructure.Repository;

namespace UserService.API
{
	public class Startup
	{
		private readonly IConfiguration _configuration;

		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<UserServiceDbContext>(options =>
			{
				options.UseNpgsql(_configuration.GetConnectionString("Postgres"));
			});
			services.Configure<AuthorizationOptions>(_configuration.GetSection(nameof(AuthorizationOptions)));

			services.Configure<JwtOptions>(_configuration.GetSection(nameof(JwtOptions)));
			var jwtOptions = _configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

			services.AddAuthorization();
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero,

					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),

					NameClaimType = JwtRegisteredClaimNames.Sub
				};
			});

			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Please enter a valid token",
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					BearerFormat = "JWT",
					Scheme = "Bearer"
				});
				options.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type=ReferenceType.SecurityScheme,
								Id="Bearer"
							}
						},
						new string[]{}
					}
				});
			});

			services.AddControllers();
			services.AddSwaggerGen();

			services.AddScoped<IUserService, Domain.Services.UserService>();
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IAuthRepository, AuthRepository>();
			services.AddScoped<IPermissionService, PermissionService>();

			services.AddSingleton<ITokenService, TokenService>();
			services.AddSingleton<IPasswordService, PasswordService>();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Service");
				});
			}

			using (var scope = app.ApplicationServices.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<UserServiceDbContext>();
				dbContext.Database.Migrate();
			}

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
