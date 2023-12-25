using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeGenerator.Migrations
{
    public partial class addEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnumTypeId",
                table: "Fields",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EnumTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnumTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnumFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnumTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnumFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnumFields_EnumTypes_EnumTypeId",
                        column: x => x.EnumTypeId,
                        principalTable: "EnumTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fields_EnumTypeId",
                table: "Fields",
                column: "EnumTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EnumFields_EnumTypeId",
                table: "EnumFields",
                column: "EnumTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fields_EnumTypes_EnumTypeId",
                table: "Fields",
                column: "EnumTypeId",
                principalTable: "EnumTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fields_EnumTypes_EnumTypeId",
                table: "Fields");

            migrationBuilder.DropTable(
                name: "EnumFields");

            migrationBuilder.DropTable(
                name: "EnumTypes");

            migrationBuilder.DropIndex(
                name: "IX_Fields_EnumTypeId",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "EnumTypeId",
                table: "Fields");
        }
    }
}
