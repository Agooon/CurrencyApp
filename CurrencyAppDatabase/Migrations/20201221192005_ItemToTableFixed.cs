using Microsoft.EntityFrameworkCore.Migrations;

namespace CurrencyAppDatabase.Migrations
{
    public partial class ItemToTableFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemTables_ItemTableId",
                table: "Items");

            migrationBuilder.AlterColumn<int>(
                name: "ItemTableId",
                table: "Items",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemTables_ItemTableId",
                table: "Items",
                column: "ItemTableId",
                principalTable: "ItemTables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemTables_ItemTableId",
                table: "Items");

            migrationBuilder.AlterColumn<int>(
                name: "ItemTableId",
                table: "Items",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemTables_ItemTableId",
                table: "Items",
                column: "ItemTableId",
                principalTable: "ItemTables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
