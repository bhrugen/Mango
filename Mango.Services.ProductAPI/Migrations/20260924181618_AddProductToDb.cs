using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mango.ProductAPI.Migrations;

/// <inheritdoc />
public partial class _20260924181618_AddProductToDb : Migration
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
                ImageFileName = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Products", x => x.ProductId);
            });

        migrationBuilder.InsertData(
            table: "Products",
            columns: new[] { "ProductId", "CategoryName", "Description", "ImageFileName", "Name", "Price" },
            values: new object[,]
            {
                { 1, "Appetizer", "Grilled bread rubbed with garlic and topped with fresh tomato, basil, and olive oil.", null, "Bruschetta", 8.9900000000000002 },
                { 2, "Appetizer", "Sliced tomatoes and fresh mozzarella layered with basil and a balsamic drizzle.", null, "Caprese Salad", 9.9900000000000002 },
                { 3, "Appetizer", "Oven-baked Italian flatbread with rosemary, sea salt, and olive oil.", null, "Focaccia Bread", 6.9900000000000002 },
                { 4, "Main Course", "Classic Roman pasta tossed with Pecorino Romano and cracked black pepper.", null, "Cacio e Pepe", 15.99 },
                { 5, "Main Course", "Wood-fired pizza with San Marzano tomatoes, fresh mozzarella, and basil.", null, "Margherita Pizza", 14.99 },
                { 6, "Main Course", "Pasta tossed in a vibrant basil pesto with pine nuts and Parmesan.", null, "Pesto Pasta", 15.49 },
                { 7, "Main Course", "Wood-fired pizza topped with basil pesto, mozzarella, and cherry tomatoes.", null, "Pesto Pizza", 16.489999999999998 },
                { 8, "Main Course", "Pasta in a rich San Marzano tomato sauce with fresh basil and garlic.", null, "Pomodoro Pasta", 13.99 },
                { 9, "Main Course", "Handmade ravioli filled with ricotta and spinach in a sage butter sauce.", null, "Spinach Ravioli", 16.989999999999998 },
                { 10, "Dessert", "Crisp pastry shell filled with sweetened ricotta and chocolate chips.", null, "Cannolo", 7.9900000000000002 },
                { 11, "Dessert", "Silky vanilla bean panna cotta topped with a fresh berry compote.", null, "Panna Cotta", 7.4900000000000002 },
                { 12, "Dessert", "Layers of espresso-soaked ladyfingers and mascarpone cream, dusted with cocoa.", null, "Tiramisu", 8.4900000000000002 }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Products");
    }
}
