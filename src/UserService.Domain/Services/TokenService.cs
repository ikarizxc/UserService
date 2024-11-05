using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserService.Domain.Interfaces.Services;
using UserService.Domain.Models;
using UserService.Domain.Options;

namespace UserService.Domain.Services
{
	public class TokenService : ITokenService
	{
		private readonly IOptions<JwtOptions> _jwtOptions;

        public TokenService(IOptions<JwtOptions> jwtOptions)
        {
			_jwtOptions = jwtOptions;
		}

        public string GenerateAccessToken(User user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();

			var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Value.Key));

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[] { new Claim("sub", user.Id.ToString()) }),
				Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(_jwtOptions.Value.LifeTimeMin)),
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
