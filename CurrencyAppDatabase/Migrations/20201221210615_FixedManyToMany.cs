using Microsoft.EntityFrameworkCore.Migrations;

namespace CurrencyAppDatabase.Migrations
{
    public partial class FixedManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTables_AspNetUsers_TableId",
                table: "UserTables");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTables_ItemTables_UserId",
                table: "UserTables");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTables_ItemTables_TableId",
                table: "UserTables",
                column: "TableId",
                principalTable: "ItemTables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTables_AspNetUsers_UserId",
                table: "UserTables",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTables_ItemTables_TableId",
                table: "UserTables");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTables_AspNetUsers_UserId",
                table: "UserTables");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTables_AspNetUsers_TableId",
                table: "UserTables",
                column: "TableId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTables_ItemTables_UserId",
                table: "UserTables",
                column: "UserId",
                principalTable: "ItemTables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
