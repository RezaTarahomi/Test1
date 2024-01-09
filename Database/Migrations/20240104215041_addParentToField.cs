using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeGenerator.Migrations
{
    public partial class addParentToField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityParents");

            migrationBuilder.AddColumn<int>(
                name: "ForeignKeyId",
                table: "Fields",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsForeignKey",
                table: "Fields",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOneByOne",
                table: "Fields",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsParent",
                table: "Fields",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Fields",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fields_ForeignKeyId",
                table: "Fields",
                column: "ForeignKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_Fields_ParentId",
                table: "Fields",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fields_Entities_ParentId",
                table: "Fields",
                column: "ParentId",
                principalTable: "Entities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fields_Fields_ForeignKeyId",
                table: "Fields",
                column: "ForeignKeyId",
                principalTable: "Fields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fields_Entities_ParentId",
                table: "Fields");

            migrationBuilder.DropForeignKey(
                name: "FK_Fields_Fields_ForeignKeyId",
                table: "Fields");

            migrationBuilder.DropIndex(
                name: "IX_Fields_ForeignKeyId",
                table: "Fields");

            migrationBuilder.DropIndex(
                name: "IX_Fields_ParentId",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "ForeignKeyId",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "IsForeignKey",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "IsOneByOne",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "IsParent",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Fields");

            migrationBuilder.CreateTable(
                name: "EntityParents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    ForeginKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OneToOne = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityParents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityParents_Entities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EntityParents_Entities_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Entities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntityParents_EntityId",
                table: "EntityParents",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityParents_ParentId",
                table: "EntityParents",
                column: "ParentId");
        }
    }
}
