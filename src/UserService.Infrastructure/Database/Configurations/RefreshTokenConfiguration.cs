using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Database.Configurations
{
	public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
	{
		public void Configure(EntityTypeBuilder<RefreshToken> builder)
		{
			builder
				.HasKey(x => x.Id);

			builder
				.Property(x => x.AccessToken)
				.IsRequired();

			builder
				.Property(x => x.UserId)
				.IsRequired();

			builder
				.Property(x => x.ExpiryDate)
				.IsRequired();

			builder
				.Property(x => x.CreationDate)
				.IsRequired();
		}
	}
}
