using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UserService.Domain.Enums;

namespace UserService.API.AuthorizePermissionExtension
{
	public class AuthorizePermissionAttribute : TypeFilterAttribute
	{
		public AuthorizePermissionAttribute(params PermissionEnum[] permissions) : base(typeof(AuthorizePermissionFilter))
		{
			Arguments = new object[] { permissions };
		}
	}
}
