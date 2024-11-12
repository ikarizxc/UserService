using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Enums;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Database.Configurations
{
	public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
	{
		public void Configure(EntityTypeBuilder<Permission> builder)
		{
			builder.HasKey(x => x.Id);

			var permissions = Enum
				.GetValues<Permissions>()
				.Select(x => new Permission
				{
					Id = (int)x,
					Name = x.ToString(),
				});

			builder.HasData(permissions);
		}
	}
}
