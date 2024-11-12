using UserService.Domain.Enums;
using UserService.Domain.Models;

namespace UserService.Domain.Interfaces.Repositories
{
	public interface IUserRepository
	{
		Task AddAsync(User user, CancellationToken cancellationToken);

		Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

		Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);

		Task<List<User>> GetAllAsync(CancellationToken cancellationToken);

		Task DeleteAsync(User user, CancellationToken cancellationToken);

		Task UpdateAsync(User user, CancellationToken cancellationToken);

		Task<HashSet<PermissionEnum>> GetPermissions(Guid userId);
	}
}
