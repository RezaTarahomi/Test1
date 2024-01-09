using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeGenerator.Migrations
{
    public partial class addisNullableField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNullable",
                table: "Fields",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNullable",
                table: "Fields");
        }
    }
}
