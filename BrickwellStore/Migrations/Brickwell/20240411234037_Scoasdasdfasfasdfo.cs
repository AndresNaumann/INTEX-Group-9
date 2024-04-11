using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrickwellStore.Migrations.Brickwell
{
    /// <inheritdoc />
    public partial class Scoasdasdfasfasdfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "LineItems",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "LineItems");
        }
    }
}
