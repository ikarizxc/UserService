﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Domain.Options
{
	public class AuthorizationOptions
	{
		public RolePermissions[] RolePermissions { get; set; } = [];
    }

	public class RolePermissions
	{
		public string Role { get; set; } = string.Empty;
		public string[] Permissions { get; set; } = [];
    }
}
