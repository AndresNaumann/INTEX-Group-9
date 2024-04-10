using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrickwellStore.Migrations.Brickwell
{
    /// <inheritdoc />
    public partial class recommendationsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerRecommendations",
                columns: table => new
                {
                    customerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    recommendedProductId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerRecommendations", x => x.customerId);
                });

            migrationBuilder.CreateTable(
                name: "ProductRecommendations",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecommendedProductID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRecommendations", x => x.ProductID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerRecommendations");

            migrationBuilder.DropTable(
                name: "ProductRecommendations");
        }
    }
}
