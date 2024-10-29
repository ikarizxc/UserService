﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Database.Configurations
{
	public class UsersConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder
				.HasKey(x => x.Id);

			builder
				.Property(x => x.Username)
				.IsRequired();

			builder
				.Property(x => x.PasswordHash)
				.IsRequired();

			builder
				.Property(x => x.CreatedAt)
				.IsRequired();
		}
	}
}