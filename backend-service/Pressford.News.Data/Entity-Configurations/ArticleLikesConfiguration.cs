using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pressford.News.Entities;

namespace Pressford.News.Data
{
    public class ArticleLikesConfiguration : IEntityTypeConfiguration<ArticleLikes>
    {
        public void Configure(EntityTypeBuilder<ArticleLikes> builder)
        {
            builder.HasKey(x => x.LikeId);
        }
    }
}