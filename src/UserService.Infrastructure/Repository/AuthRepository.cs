using Microsoft.EntityFrameworkCore;
using UserService.Domain.Interfaces.Repositories;
using UserService.Domain.Models;
using UserService.Infrastructure.Database;

namespace UserService.Infrastructure.Repository
{
	public class AuthRepository : IAuthRepository
	{
		private readonly UserServiceDbContext _dbContext;
		public AuthRepository(UserServiceDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
		{
			await _dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}

		public async Task DeleteAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
		{
			_dbContext.RefreshTokens.Remove(refreshToken);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}

		public async Task<RefreshToken?> GetAsync(string token, CancellationToken cancellationToken)
		{
			return await _dbContext.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
		}

		public async Task<RefreshToken?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
		{
			return await _dbContext.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
		}

		public async Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
		{
			_dbContext.RefreshTokens.Update(refreshToken);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
