using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pressford.News.Entities;

namespace Pressford.News.Data
{
	public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
	{
		public void Configure(EntityTypeBuilder<UserLogin> builder)
		{
			builder.HasKey(x => x.Username);
			builder.Property(x => x.Password).IsRequired();
			builder.Property(x => x.Role)
						.HasConversion(new EnumToStringConverter<RoleType>());
		}
	}
}