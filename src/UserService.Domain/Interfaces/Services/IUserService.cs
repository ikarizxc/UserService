using UserService.Domain.DTOs;

namespace UserService.Domain.Interfaces.Services
{
	public interface IUserService
	{
		Task<UserDTO> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);

		Task<List<UserDTO>> GetUsersAsync(CancellationToken cancellationToken);

		Task CreateUserAsync(UserCreateDTO userDTO, CancellationToken cancellationToken);

		Task DeleteUserAsync(Guid id, CancellationToken cancellationToken);

		Task UpdateUserAsync(UserDTO userDTO, CancellationToken cancellationToken);
	}
}
