using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.ProductAPI.Migrations;

/// <inheritdoc />
public partial class _20260924014006_RemoveSeedImageUrls : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 1,
            column: "ImageUrl",
            value: null);

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 2,
            column: "ImageUrl",
            value: null);

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 3,
            column: "ImageUrl",
            value: null);

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 4,
            column: "ImageUrl",
            value: null);

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 5,
            column: "ImageUrl",
            value: null);

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 6,
            column: "ImageUrl",
            value: null);

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 7,
            column: "ImageUrl",
            value: null);

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 8,
            column: "ImageUrl",
            value: null);

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 9,
            column: "ImageUrl",
            value: null);

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 10,
            column: "ImageUrl",
            value: null);

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 11,
            column: "ImageUrl",
            value: null);

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 12,
            column: "ImageUrl",
            value: null);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 1,
            column: "ImageUrl",
            value: "https://localhost:7003/ProductImages/a-bruschetta.png");

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 2,
            column: "ImageUrl",
            value: "https://localhost:7003/ProductImages/a-caprese-salad.png");

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 3,
            column: "ImageUrl",
            value: "https://localhost:7003/ProductImages/a-focaccia-bread.png");

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 4,
            column: "ImageUrl",
            value: "https://localhost:7003/ProductImages/e-cacio-pepe.png");

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 5,
            column: "ImageUrl",
            value: "https://localhost:7003/ProductImages/e-margherita-pizza.png");

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 6,
            column: "ImageUrl",
            value: "https://localhost:7003/ProductImages/e-pesto-pasta.png");

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 7,
            column: "ImageUrl",
            value: "https://localhost:7003/ProductImages/e-pesto-pizza.png");

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 8,
            column: "ImageUrl",
            value: "https://localhost:7003/ProductImages/e-pomodoro-pasta.png");

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 9,
            column: "ImageUrl",
            value: "https://localhost:7003/ProductImages/e-spinach-ravioli.png");

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 10,
            column: "ImageUrl",
            value: "https://localhost:7003/ProductImages/d-cannolo.png");

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 11,
            column: "ImageUrl",
            value: "https://localhost:7003/ProductImages/d-panna-cotta.png");

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 12,
            column: "ImageUrl",
            value: "https://localhost:7003/ProductImages/d-tiramisu.png");
    }
}
