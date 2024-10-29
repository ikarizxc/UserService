using Microsoft.AspNetCore.Identity;
using UserService.Domain.DTOs;
using UserService.Domain.Interfaces.Repositories;
using UserService.Domain.Interfaces.Services;
using UserService.Domain.Models;

namespace UserService.Services
{
	public class AuthService : IAuthService
	{
		private readonly IUserRepository _userRepository;
		private readonly IAuthRepository _authRepository;
		private readonly ITokenService _tokenService;
		private readonly IPasswordService _passwordService;

		public AuthService(
			IUserRepository userRepository, 
			ITokenService tokenService, 
			IPasswordService passwordService,
			IAuthRepository authRepository)
        {
            _userRepository = userRepository;
			_tokenService = tokenService;
			_passwordService = passwordService;
			_authRepository = authRepository;
		}

        public async Task<TokenResponseDTO?> GenerateTokenAsync(UserLoginDTO userLoginDTO, CancellationToken cancellationToken)
		{
			var user = await _userRepository.GetByUsernameAsync(userLoginDTO.Username, cancellationToken);
			if (user == null)
			{
				return null;
			}

			var isCorrect = _passwordService.VerifyPassword(userLoginDTO.Password, user.PasswordHash);
			if (!isCorrect)
			{
				return null;
			}

			var oldRefreshToken = await _authRepository.GetByUserIdAsync(user.Id, cancellationToken);
			if (oldRefreshToken != null)
			{
				await _authRepository.DeleteAsync(oldRefreshToken, cancellationToken);
			}

			var accessToken = _tokenService.GenerateAccessToken(user);
			var refreshToken = new RefreshToken()
			{
				Id = Guid.NewGuid(),
				Token = _tokenService.GenerateRefreshToken(),
				AccessToken = accessToken,
				CreationDate = DateTime.UtcNow,
				ExpiryDate = DateTime.UtcNow.Add(TimeSpan.FromDays(30)),
				UserId = user.Id,
			};

			await _authRepository.AddAsync(refreshToken, cancellationToken);

			return new TokenResponseDTO()
			{
				AccessToken = accessToken,
				RefreshToken = refreshToken.Token,
				Type = "Bearer"
			};
		}

		public async Task<TokenResponseDTO?> RefreshTokenAsync(RefreshTokenDTO refreshTokenDTO, CancellationToken cancellationToken)
		{
			var refreshToken = await _authRepository.GetAsync(refreshTokenDTO.RefreshToken, cancellationToken);
			if (refreshToken == null)
			{
				return null;
			}

			if (refreshToken.ExpiryDate < DateTime.UtcNow)
			{
				await _authRepository.DeleteAsync(refreshToken, cancellationToken);
				return null;
			}

			var user = await _userRepository.GetByIdAsync(refreshToken.UserId, cancellationToken);
			if (user == null)
			{
				await _authRepository.DeleteAsync(refreshToken, cancellationToken);
				return null;
			}

			var accessToken = _tokenService.GenerateAccessToken(user);

			refreshToken.Token = _tokenService.GenerateRefreshToken();
			refreshToken.CreationDate = DateTime.UtcNow;
			refreshToken.ExpiryDate = DateTime.UtcNow.Add(TimeSpan.FromDays(30));
			refreshToken.AccessToken = accessToken;

			await _authRepository.UpdateAsync(refreshToken, cancellationToken);

			return new TokenResponseDTO()
			{
				AccessToken = accessToken,
				RefreshToken = refreshToken.Token,
				Type = "Bearer"
			};
		}
	}
}
