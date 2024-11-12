using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using UserService.Domain.Interfaces.Services;
using UserService.Domain.Enums;
using System.Security.Claims;

namespace UserService.API.AuthorizePermissionExtension
{
	public class AuthorizePermissionFilter : IAsyncActionFilter
	{
		private readonly PermissionEnum[] _permissions;
		private readonly IPermissionService _permissionService;

		public AuthorizePermissionFilter(PermissionEnum[] permissions, IPermissionService permissionService)
		{
			_permissions = permissions;
			_permissionService = permissionService;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			// Получаем UserId из токена
			var userIdClaim = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userIdClaim == null)
			{
				context.Result = new UnauthorizedResult();
				return;
			}

			var userId = Guid.Parse(userIdClaim);

			// Проверка, есть ли у пользователя необходимые права
			var userPermissions = await _permissionService.GetPermissions(userId);

			if (!userPermissions.Intersect(_permissions).Any())
			{
				context.Result = new ForbidResult();
				return;
			}

			await next();
		}
	}
}
