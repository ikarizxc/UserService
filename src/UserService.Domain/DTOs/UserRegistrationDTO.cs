﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Domain.DTOs
{
	public class UserRegistrationDTO
	{
		public string Username { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
	}
}
