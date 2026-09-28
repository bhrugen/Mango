using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.OrderAPI.Migrations;

/// <inheritdoc />
public partial class _20260928213904_updateOrderTotalColumn : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "CartTotal",
            table: "OrderHeader",
            newName: "OrderTotal");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "OrderTotal",
            table: "OrderHeader",
            newName: "CartTotal");
    }
}
