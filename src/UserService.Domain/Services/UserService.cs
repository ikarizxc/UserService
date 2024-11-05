using UserService.Domain.DTOs;
using UserService.Domain.Interfaces.Repositories;
using UserService.Domain.Interfaces.Services;
using UserService.Domain.Models;

namespace UserService.Domain.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IPasswordService _passwordHasher;

		public UserService(IUserRepository userRepository, IPasswordService passwordHasher)
		{
			_userRepository = userRepository;
			_passwordHasher = passwordHasher;
		}

		public async Task<bool> CreateUserAsync(UserCreateDTO userDTO, CancellationToken cancellationToken)
		{
			var existedUser = await _userRepository.GetByUsernameAsync(userDTO.Username, cancellationToken);
			if (existedUser != null)
			{
				return false;
			}

			var user = new User()
			{
				Id = Guid.NewGuid(),
				Username = userDTO.Username,
				PasswordHash = _passwordHasher.HashPassword(userDTO.Password),
				CreatedAt = DateTime.UtcNow,
			};

			await _userRepository.AddAsync(user, cancellationToken); 
			return true;
		}

		public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken)
		{
			var user = await _userRepository.GetByIdAsync(id, cancellationToken);
			if (user == null)
			{
				return;
			}

			await _userRepository.DeleteAsync(user, cancellationToken);
		}

		public async Task<UserDTO?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			var user = await _userRepository.GetByIdAsync(id, cancellationToken);

			if (user == null)
			{
				return null;
			}

			return new UserDTO()
			{
				Id = user.Id,
				Username = user.Username,
			};
		}

		public async Task<List<UserDTO>> GetUsersAsync(CancellationToken cancellationToken)
		{
			return (await _userRepository.GetAllAsync(cancellationToken))
				.Select(x => new UserDTO() { Id = x.Id, Username = x.Username }).ToList();
		}

		public async Task UpdateUserAsync(UserDTO userDTO, CancellationToken cancellationToken)
		{
			var user = await _userRepository.GetByIdAsync(userDTO.Id, cancellationToken);
			if (user == null)
			{
				return;
			}

			var userWithUsername = await _userRepository.GetByUsernameAsync(userDTO.Username, cancellationToken);
			if (userWithUsername != null)
			{
				return;
			}

			user.Username = userDTO.Username;
			await _userRepository.UpdateAsync(user, cancellationToken);
		}
	}
}
