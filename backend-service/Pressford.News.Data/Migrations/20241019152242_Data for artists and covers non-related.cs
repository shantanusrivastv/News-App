using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pressford.News.Data.Migrations
{
    /// <inheritdoc />
    public partial class Dataforartistsandcoversnonrelated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Artist",
                columns: new[] { "ArtistId", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, "John", "Doe" },
                    { 2, "Jane", "Smith" },
                    { 3, "Michael", "Johnson" }
                });

            migrationBuilder.InsertData(
                table: "Cover",
                columns: new[] { "CoverId", "DesignIdeas", "DigitalOnly" },
                values: new object[,]
                {
                    { 1, "Left hand in dark", true },
                    { 2, "Add a clock", true },
                    { 3, "Massive Cloud in dark background", false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Artist",
                keyColumn: "ArtistId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Artist",
                keyColumn: "ArtistId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Artist",
                keyColumn: "ArtistId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cover",
                keyColumn: "CoverId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cover",
                keyColumn: "CoverId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cover",
                keyColumn: "CoverId",
                keyValue: 3);
        }
    }
}
