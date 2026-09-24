using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.ProductAPI.Migrations;

/// <inheritdoc />
public partial class _20260924014421_ReplaceImageUrlWithFileName : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "ImageLocalPath",
            table: "Products");

        migrationBuilder.RenameColumn(
            name: "ImageUrl",
            table: "Products",
            newName: "ImageFileName");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "ImageFileName",
            table: "Products",
            newName: "ImageUrl");

        migrationBuilder.AddColumn<string>(
            name: "ImageLocalPath",
            table: "Products",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 1,
            column: "ImageLocalPath",
            value: null);

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 2,
            column: "ImageLocalPath",
            value: null);

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 3,
            column: "ImageLocalPath",
            value: null);

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 4,
            column: "ImageLocalPath",
            value: null);

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 5,
            column: "ImageLocalPath",
            value: null);

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 6,
            column: "ImageLocalPath",
            value: null);

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 7,
            column: "ImageLocalPath",
            value: null);

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 8,
            column: "ImageLocalPath",
            value: null);

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 9,
            column: "ImageLocalPath",
            value: null);

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 10,
            column: "ImageLocalPath",
            value: null);

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 11,
            column: "ImageLocalPath",
            value: null);

        migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 12,
            column: "ImageLocalPath",
            value: null);
    }
}
