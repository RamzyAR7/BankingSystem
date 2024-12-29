using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank_System_PaySky.Migrations
{
    /// <inheritdoc />
    public partial class m2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "AccountTransactions",
                newName: "TransactionCurrancy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionCurrancy",
                table: "AccountTransactions",
                newName: "CurrencyCode");
        }
    }
}
