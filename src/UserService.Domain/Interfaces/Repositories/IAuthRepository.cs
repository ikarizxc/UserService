using UserService.Domain.Models;

namespace UserService.Domain.Interfaces.Repositories
{
	public interface IAuthRepository
	{
		Task<RefreshToken?> GetAsync(string token, CancellationToken cancellationToken);
		Task<RefreshToken?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
		Task DeleteAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
		Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
		Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
	}
}
