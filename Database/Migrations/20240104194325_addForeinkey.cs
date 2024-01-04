using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeGenerator.Migrations
{
    public partial class addForeinkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ForeginKey",
                table: "EntityParents",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForeginKey",
                table: "EntityParents");
        }
    }
}
