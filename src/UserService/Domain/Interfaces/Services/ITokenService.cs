using UserService.Domain.Models;

namespace UserService.Domain.Interfaces.Services
{
	public interface ITokenService
	{
		string GenerateAccessToken(User user);

		string GenerateRefreshToken();
	}
}
