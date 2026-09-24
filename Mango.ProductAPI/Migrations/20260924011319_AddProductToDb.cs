using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mango.ProductAPI.Migrations;

/// <inheritdoc />
public partial class _20260924011319_AddProductToDb : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Products",
            columns: table => new
            {
                ProductId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Price = table.Column<double>(type: "float", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ImageLocalPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Products", x => x.ProductId);
            });

        migrationBuilder.InsertData(
            table: "Products",
            columns: new[] { "ProductId", "CategoryName", "Description", "ImageLocalPath", "ImageUrl", "Name", "Price" },
            values: new object[,]
            {
                { 1, "Main Course", "Tender chicken tikka glazed with mango chutney.", null, null, "Mango Chicken Tikka", 14.99 },
                { 2, "Beverage", "Chilled yogurt drink blended with fresh mango.", null, null, "Mango Lassi", 4.9900000000000002 }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Products");
    }
}
