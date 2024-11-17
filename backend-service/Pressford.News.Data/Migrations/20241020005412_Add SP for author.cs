using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pressford.News.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSPforauthor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE dbo.AuthorsArticlesInYearRange
                     @yearStart datetime,
                     @yearEnd datetime
                  AS
                  SELECT Distinct usr.* FROM dbo.[User] usr
                  LEFT JOIN Article art ON art.Author = usr.Email
                  WHERE art.DatePublished between @yearStart and @yearEnd;");
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"
                DROP PROCEDURE dbo.AuthorsArticlesInYearRange");
		}
    }
}
