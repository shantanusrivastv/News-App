using Microsoft.EntityFrameworkCore.Migrations;

namespace Pressford.News.Data.Migrations
{
	public partial class SeedUserandlogindata : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.InsertData(
				table: "User",
				columns: new[] { "Id", "Email", "FirstName", "LastName" },
				values: new object[] { 1, "w.Pressford@pressford.com", "W", "Pressford" });

			migrationBuilder.InsertData(
				table: "User",
				columns: new[] { "Id", "Email", "FirstName", "LastName" },
				values: new object[] { 2, "adminUser@pressford.com", "Admin", "User" });

			migrationBuilder.InsertData(
				table: "User",
				columns: new[] { "Id", "Email", "FirstName", "LastName" },
				values: new object[] { 3, "normalUser@pressford.com", "Normal", "User" });

			migrationBuilder.InsertData(
				table: "UserLogin",
				columns: new[] { "Username", "Password", "Role" },
				values: new object[] { "w.Pressford@pressford.com", "admin", "Publisher" });

			migrationBuilder.InsertData(
				table: "UserLogin",
				columns: new[] { "Username", "Password", "Role" },
				values: new object[] { "adminUser@pressford.com", "admin", "Publisher" });

			migrationBuilder.InsertData(
				table: "UserLogin",
				columns: new[] { "Username", "Password", "Role" },
				values: new object[] { "normalUser@pressford.com", "user", "User" });
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DeleteData(
				table: "UserLogin",
				keyColumn: "Username",
				keyValue: "adminUser@pressford.com");

			migrationBuilder.DeleteData(
				table: "UserLogin",
				keyColumn: "Username",
				keyValue: "normalUser@pressford.com");

			migrationBuilder.DeleteData(
				table: "UserLogin",
				keyColumn: "Username",
				keyValue: "w.Pressford@pressford.com");

			migrationBuilder.DeleteData(
				table: "User",
				keyColumn: "Id",
				keyValue: 1);

			migrationBuilder.DeleteData(
				table: "User",
				keyColumn: "Id",
				keyValue: 2);

			migrationBuilder.DeleteData(
				table: "User",
				keyColumn: "Id",
				keyValue: 3);
		}
	}
}