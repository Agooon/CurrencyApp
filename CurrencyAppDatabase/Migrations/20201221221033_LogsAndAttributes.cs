using Microsoft.EntityFrameworkCore.Migrations;

namespace CurrencyAppDatabase.Migrations
{
    public partial class LogsAndAttributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Items",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyTo",
                table: "Items",
                type: "VARCHAR(8)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyFrom",
                table: "Items",
                type: "VARCHAR(8)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyTo",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(8)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyFrom",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(8)",
                oldNullable: true);
        }
    }
}
