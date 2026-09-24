using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mango.ProductAPI.Migrations;

/// <inheritdoc />
public partial class _20260924013835_SeedProductMenu : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 1,
            columns: new[] { "CategoryName", "Description", "ImageUrl", "Name", "Price" },
            values: new object[] { "Appetizer", "Grilled bread rubbed with garlic and topped with fresh tomato, basil, and olive oil.", "https://localhost:7003/ProductImages/a-bruschetta.png", "Bruschetta", 8.9900000000000002 });

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 2,
            columns: new[] { "CategoryName", "Description", "ImageUrl", "Name", "Price" },
            values: new object[] { "Appetizer", "Sliced tomatoes and fresh mozzarella layered with basil and a balsamic drizzle.", "https://localhost:7003/ProductImages/a-caprese-salad.png", "Caprese Salad", 9.9900000000000002 });

        migrationBuilder.InsertData(
            table: "Products",
            columns: new[] { "ProductId", "CategoryName", "Description", "ImageLocalPath", "ImageUrl", "Name", "Price" },
            values: new object[,]
            {
                { 3, "Appetizer", "Oven-baked Italian flatbread with rosemary, sea salt, and olive oil.", null, "https://localhost:7003/ProductImages/a-focaccia-bread.png", "Focaccia Bread", 6.9900000000000002 },
                { 4, "Main Course", "Classic Roman pasta tossed with Pecorino Romano and cracked black pepper.", null, "https://localhost:7003/ProductImages/e-cacio-pepe.png", "Cacio e Pepe", 15.99 },
                { 5, "Main Course", "Wood-fired pizza with San Marzano tomatoes, fresh mozzarella, and basil.", null, "https://localhost:7003/ProductImages/e-margherita-pizza.png", "Margherita Pizza", 14.99 },
                { 6, "Main Course", "Pasta tossed in a vibrant basil pesto with pine nuts and Parmesan.", null, "https://localhost:7003/ProductImages/e-pesto-pasta.png", "Pesto Pasta", 15.49 },
                { 7, "Main Course", "Wood-fired pizza topped with basil pesto, mozzarella, and cherry tomatoes.", null, "https://localhost:7003/ProductImages/e-pesto-pizza.png", "Pesto Pizza", 16.489999999999998 },
                { 8, "Main Course", "Pasta in a rich San Marzano tomato sauce with fresh basil and garlic.", null, "https://localhost:7003/ProductImages/e-pomodoro-pasta.png", "Pomodoro Pasta", 13.99 },
                { 9, "Main Course", "Handmade ravioli filled with ricotta and spinach in a sage butter sauce.", null, "https://localhost:7003/ProductImages/e-spinach-ravioli.png", "Spinach Ravioli", 16.989999999999998 },
                { 10, "Dessert", "Crisp pastry shell filled with sweetened ricotta and chocolate chips.", null, "https://localhost:7003/ProductImages/d-cannolo.png", "Cannolo", 7.9900000000000002 },
                { 11, "Dessert", "Silky vanilla bean panna cotta topped with a fresh berry compote.", null, "https://localhost:7003/ProductImages/d-panna-cotta.png", "Panna Cotta", 7.4900000000000002 },
                { 12, "Dessert", "Layers of espresso-soaked ladyfingers and mascarpone cream, dusted with cocoa.", null, "https://localhost:7003/ProductImages/d-tiramisu.png", "Tiramisu", 8.4900000000000002 }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 3);

        migrationBuilder.DeleteData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 4);

        migrationBuilder.DeleteData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 5);

        migrationBuilder.DeleteData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 6);

        migrationBuilder.DeleteData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 7);

        migrationBuilder.DeleteData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 8);

        migrationBuilder.DeleteData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 9);

        migrationBuilder.DeleteData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 10);

        migrationBuilder.DeleteData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 11);

        migrationBuilder.DeleteData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 12);

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 1,
            columns: new[] { "CategoryName", "Description", "ImageUrl", "Name", "Price" },
            values: new object[] { "Main Course", "Tender chicken tikka glazed with mango chutney.", null, "Mango Chicken Tikka", 14.99 });

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 2,
            columns: new[] { "CategoryName", "Description", "ImageUrl", "Name", "Price" },
            values: new object[] { "Beverage", "Chilled yogurt drink blended with fresh mango.", null, "Mango Lassi", 4.9900000000000002 });
    }
}
