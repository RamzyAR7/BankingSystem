using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank_System_PaySky.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountNumber",
                table: "Accounts",
                newName: "AccountNumbers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountNumbers",
                table: "Accounts",
                newName: "AccountNumber");
        }
    }
}
