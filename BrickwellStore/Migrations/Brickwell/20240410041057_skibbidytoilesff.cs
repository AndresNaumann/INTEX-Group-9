using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrickwellStore.Migrations.Brickwell
{
    /// <inheritdoc />
    public partial class skibbidytoilesff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductRecommendations",
                table: "ProductRecommendations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerRecommendations",
                table: "CustomerRecommendations");

            migrationBuilder.RenameColumn(
                name: "recommendedProductId",
                table: "CustomerRecommendations",
                newName: "RecommendedProductId");

            migrationBuilder.RenameColumn(
                name: "customerId",
                table: "CustomerRecommendations",
                newName: "CustomerId");

            migrationBuilder.AlterColumn<int>(
                name: "ProductID",
                table: "ProductRecommendations",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "RecId",
                table: "ProductRecommendations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "CustomerRecommendations",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "RecId",
                table: "CustomerRecommendations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductRecommendations",
                table: "ProductRecommendations",
                column: "RecId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerRecommendations",
                table: "CustomerRecommendations",
                column: "RecId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductRecommendations",
                table: "ProductRecommendations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerRecommendations",
                table: "CustomerRecommendations");

            migrationBuilder.DropColumn(
                name: "RecId",
                table: "ProductRecommendations");

            migrationBuilder.DropColumn(
                name: "RecId",
                table: "CustomerRecommendations");

            migrationBuilder.RenameColumn(
                name: "RecommendedProductId",
                table: "CustomerRecommendations",
                newName: "recommendedProductId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "CustomerRecommendations",
                newName: "customerId");

            migrationBuilder.AlterColumn<int>(
                name: "ProductID",
                table: "ProductRecommendations",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "customerId",
                table: "CustomerRecommendations",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductRecommendations",
                table: "ProductRecommendations",
                column: "ProductID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerRecommendations",
                table: "CustomerRecommendations",
                column: "customerId");
        }
    }
}
