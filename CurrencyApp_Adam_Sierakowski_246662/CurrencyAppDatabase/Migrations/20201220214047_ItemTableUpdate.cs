using Microsoft.EntityFrameworkCore.Migrations;

namespace CurrencyAppDatabase.Migrations
{
    public partial class ItemTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemTableId",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Items",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemTableId",
                table: "Items",
                column: "ItemTableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemTables_ItemTableId",
                table: "Items",
                column: "ItemTableId",
                principalTable: "ItemTables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemTables_ItemTableId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ItemTableId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemTableId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Items");
        }
    }
}
