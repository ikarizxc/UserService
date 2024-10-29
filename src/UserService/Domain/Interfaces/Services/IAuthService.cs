using UserService.Domain.DTOs;
using UserService.Domain.Models;

namespace UserService.Domain.Interfaces.Services
{
	public interface IAuthService
	{
		Task<TokenResponseDTO?> GenerateTokenAsync(UserLoginDTO userLoginDTO, CancellationToken cancellationToken);
		Task<TokenResponseDTO?> RefreshTokenAsync(RefreshTokenDTO token, CancellationToken cancellationToken);
	}
}
