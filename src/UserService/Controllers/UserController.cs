using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserService.Domain.DTOs;
using UserService.Domain.Interfaces.Services;

namespace UserService.Controllers
{
	[Authorize]
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
			var result = await _userService.CreateUserAsync(user, cancellationToken);
			return result ?
				Ok() : Conflict();
		}

		[HttpGet("users/{id:guid}")]
		public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
		{
			var user = await _userService.GetUserByIdAsync(id, cancellationToken);
			if (user == null)
			{
				return NotFound();
			}

			return Ok(user);
		}

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
			await _userService.DeleteUserAsync(id, cancellationToken);
			return Ok();
		}


		[HttpPut("users")]
		public async Task<IActionResult> Update([FromBody] UserDTO user, CancellationToken cancellationToken)
		{
			await _userService.UpdateUserAsync(user, cancellationToken);
			return Ok();
		}
	}
}
