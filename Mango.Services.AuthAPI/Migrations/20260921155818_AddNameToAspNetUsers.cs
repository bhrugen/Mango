using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.AuthAPI.Migrations;

/// <inheritdoc />
public partial class _20260921155818_AddNameToAspNetUsers : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Name",
            table: "AspNetUsers",
            type: "nvarchar(max)",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Name",
            table: "AspNetUsers");
    }
}
