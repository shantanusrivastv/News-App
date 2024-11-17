using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pressford.News.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_DB_View : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"
                CREATE VIEW [dbo].[AuthorWithArticles]
                AS
                SELECT [User].FirstName + ' '+ [User].LastName as Author,
                        art.title as Articles
                FROM dbo.[User] 
                LEFT JOIN
                Article art ON art.Author = [User].Email");
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"DROP VIEW  AuthorWithArticles");
		}
    }
}
