using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using UserService.Domain.Enums;
using UserService.Domain.Models;
using UserService.Domain.Options;

namespace UserService.Infrastructure.Database.Configurations
{
	public class RolePermissionConfigiration : IEntityTypeConfiguration<RolePermission>
	{
		private readonly AuthorizationOptions _authOptions;

		public RolePermissionConfigiration(AuthorizationOptions authOptions)
        {
			_authOptions = authOptions;
		}

        public void Configure(EntityTypeBuilder<RolePermission> builder)
		{
			builder.HasKey(x => new {x.RoleId, x.PermissionId});

			builder.HasData(ParseRolePermissions());
		}

		private RolePermission[] ParseRolePermissions()
		{
			return _authOptions.RolePermissions
				.SelectMany(x => x.Permissions
					.Select(p => new RolePermission
					{
						RoleId = (int)Enum.Parse<Roles>(x.Role),
						PermissionId = (int)Enum.Parse<Permissions>(p),
					}))
				.ToArray();
		}
	}
}
