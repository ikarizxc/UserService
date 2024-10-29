using Microsoft.AspNetCore.Mvc;
using UserService.Domain.DTOs;
using UserService.Domain.Interfaces.Services;
using UserService.Domain.Models;

namespace UserService.Controllers
{
	[ApiController]
	[Route("api/")]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		//[HttpPost("auth/register")]
		//public async Task<IActionResult> Register([FromBody] UserRegistrationDTO userRegistrationDto, CancellationToken cancellationToken)
		//{
		//	return Ok();
		//}

		[HttpPost("auth/login")]
		public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDto, CancellationToken cancellationToken)
		{
			var token = await _authService.GenerateTokenAsync(userLoginDto, cancellationToken);
			if (token == null)
			{
				return Unauthorized();
			}
			return Ok(token);
		}

		[HttpPost("auth/refresh")]
		public async Task<IActionResult> Refresh([FromBody] RefreshTokenDTO refreshToken, CancellationToken cancellationToken)
		{
			var token = await _authService.RefreshTokenAsync(refreshToken, cancellationToken);
			if (token == null)
			{
				return Unauthorized();
			}
			return Ok(token);
		}
	}
}
