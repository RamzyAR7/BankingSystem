using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank_System_PaySky.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "Users",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "IsAdmin", "Password", "RefreshToken", "RefreshTokenExpiryTime", "UserName" },
                values: new object[] { new Guid("c81e1901-ec0c-414c-be3a-9a5693270bb0"), "admin@gmail.com", true, "$2a$11$jbWlcKbt3neZMOscxS3bIOxNWSo2kY2GnaL447VV7GCfQtoalciz.", null, null, "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("c81e1901-ec0c-414c-be3a-9a5693270bb0"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
