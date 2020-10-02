using Microsoft.EntityFrameworkCore.Migrations;

namespace Pressford.News.Data.Migrations
{
    public partial class ChangedEntitynamefromAuthorNametoAuthor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "Article");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Article",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Article");

            migrationBuilder.AddColumn<string>(
                name: "AuthorName",
                table: "Article",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}