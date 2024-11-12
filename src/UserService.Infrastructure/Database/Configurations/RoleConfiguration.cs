using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Enums;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Database.Configurations
{
	public class RoleConfiguration : IEntityTypeConfiguration<Role>
	{

		public void Configure(EntityTypeBuilder<Role> builder)
		{
			builder.HasKey(x => x.Id);

			builder.HasMany(x => x.Permissions)
				.WithMany(x => x.Roles)
				.UsingEntity<RolePermission>(
					l => l.HasOne<Permission>().WithMany().HasForeignKey(x => x.PermissionId),
					r => r.HasOne<Role>().WithMany().HasForeignKey(x => x.RoleId));

			var roles = Enum
				.GetValues<RoleEnum>()
				.Select(r => new Permission
				{
					Id = (int)r,
					Name = r.ToString()
				});

			builder.HasData(roles);
		}
	}
}
