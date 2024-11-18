using Microsoft.AspNetCore.Mvc;
using UserService.Domain.DTOs;
using UserService.Domain.Exceptions;
using UserService.Domain.Interfaces.Services;

namespace UserService.API.Controllers
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

		[HttpPost("auth/register")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public async Task<IActionResult> Register([FromBody] UserRegistrationDTO userRegistrationDto, CancellationToken cancellationToken)
		{
			try
			{
				await _authService.RegisterUserAsync(userRegistrationDto, cancellationToken);
			}
			catch (CredentialsInUseException)
			{
				return Conflict();
			}

			return Ok();
		}

		[HttpPost("auth/login")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<TokenResponseDTO>> Login([FromBody] UserLoginDTO userLoginDto, CancellationToken cancellationToken)
		{
			TokenResponseDTO token;
			try
			{
				token = await _authService.GenerateTokenAsync(userLoginDto, cancellationToken);
			}
			catch (WrongCredentialsException)
			{ 
				return BadRequest();
			}

			return Ok(token);
		}

		[HttpPost("auth/refresh")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<TokenResponseDTO>> Refresh([FromBody] RefreshTokenDTO refreshToken, CancellationToken cancellationToken)
		{
			TokenResponseDTO token;
			try
			{
				token = await _authService.RefreshTokenAsync(refreshToken, cancellationToken);
			}
			catch (WrongRefreshTokenException)
			{
				return BadRequest();
			}

			return Ok(token);
		}
	}
}
