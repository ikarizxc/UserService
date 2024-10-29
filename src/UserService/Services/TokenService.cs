using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserService.Domain.Interfaces.Services;
using UserService.Domain.Models;

namespace UserService.Services
{
	public class TokenService : ITokenService
	{
		private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
			_configuration = configuration;
		}

        public string GenerateAccessToken(User user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();

			var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));

			if (!Double.TryParse(_configuration["Jwt:LifeMinutes"], out var lifeMinutes))
			{
				lifeMinutes = 15;
			}

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[] { new Claim("sub", user.Id.ToString()) }),
				Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(lifeMinutes)),
				SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public string GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}
	}
}
