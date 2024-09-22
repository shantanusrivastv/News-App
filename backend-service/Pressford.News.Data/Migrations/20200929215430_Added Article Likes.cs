using Microsoft.EntityFrameworkCore.Migrations;

namespace Pressford.News.Data.Migrations
{
	public partial class AddedArticleLikes : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "ArticleLikes",
				columns: table => new
				{
					LikeId = table.Column<int>(nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					ArticleId = table.Column<int>(nullable: false),
					UserName = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ArticleLikes", x => x.LikeId);
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "ArticleLikes");
		}
	}
}