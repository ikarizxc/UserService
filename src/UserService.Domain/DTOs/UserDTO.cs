﻿namespace UserService.Domain.DTOs
{
	public class UserDTO
	{
		public Guid Id { get; set; }
		public string Username { get; set; } = string.Empty;
	}
}
