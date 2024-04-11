using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrickwellStore.Migrations.Brickwell
{
    /// <inheritdoc />
    public partial class Scoasdasdfasfsdfgddfao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineItems_Orders_OrderTransactionId",
                table: "LineItems");

            migrationBuilder.DropIndex(
                name: "IX_LineItems_OrderTransactionId",
                table: "LineItems");

            migrationBuilder.DropColumn(
                name: "OrderTransactionId",
                table: "LineItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderTransactionId",
                table: "LineItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LineItems_OrderTransactionId",
                table: "LineItems",
                column: "OrderTransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_LineItems_Orders_OrderTransactionId",
                table: "LineItems",
                column: "OrderTransactionId",
                principalTable: "Orders",
                principalColumn: "TransactionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
