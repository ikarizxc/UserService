using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Domain.DTOs;
using UserService.Domain.Exceptions;
using UserService.Domain.Interfaces.Services;

namespace UserService.API.Controllers
{
	[ApiController]
	[Route("api/")]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpPost("users")]
		public async Task<IActionResult> Create([FromBody] UserCreateDTO user, CancellationToken cancellationToken)
		{
			try
			{
				await _userService.CreateUserAsync(user, cancellationToken);
			}
			catch (CredentialsInUseException)
			{
				return Conflict();
			}
			
			return Ok();
		}

		[HttpGet("users/{id:guid}")]
		public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
		{
			UserDTO user;
			try
			{
				user = await _userService.GetUserByIdAsync(id, cancellationToken);
			}
			catch (NotFoundException)
			{
				return NotFound();
			}

			return Ok(user);
		}

		[Authorize]
		[HttpGet("users")]
		public async Task<IActionResult> Get(CancellationToken cancellationToken)
		{
			var users = await _userService.GetUsersAsync(cancellationToken);
			if (users.ToList().Count == 0)
			{
				return NoContent();
			}

			return Ok(users);
		}

		[HttpDelete("users/{id:guid}")]
		public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
		{
			try
			{
				await _userService.DeleteUserAsync(id, cancellationToken);
			}
			catch (NotFoundException)
			{
				return NotFound();
			}

			return Ok();
		}

		[HttpPut("users")]
		public async Task<IActionResult> Update([FromBody] UserDTO user, CancellationToken cancellationToken)
		{
			try
			{
				await _userService.UpdateUserAsync(user, cancellationToken);
			}
			catch (NotFoundException)
			{
				return NotFound();
			}
			catch(CredentialsInUseException)
			{
				return Conflict();
			}

			return Ok();
		}
	}
}
