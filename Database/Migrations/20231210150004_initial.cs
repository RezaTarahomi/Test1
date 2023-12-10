using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeGenerator.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Apis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObjectTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    ApiId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjectTypes_Apis_ApiId",
                        column: x => x.ApiId,
                        principalTable: "Apis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObjectTypes_ObjectTypes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ObjectTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RequestParameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    Filterable = table.Column<bool>(type: "bit", nullable: false),
                    Sortable = table.Column<bool>(type: "bit", nullable: false),
                    AttributeType = table.Column<byte>(type: "tinyint", nullable: true),
                    ApiId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestParameters_Apis_ApiId",
                        column: x => x.ApiId,
                        principalTable: "Apis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResponseParameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimitiveType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ObjectTypeId = table.Column<int>(type: "int", nullable: true),
                    IsList = table.Column<bool>(type: "bit", nullable: false),
                    ApiId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponseParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResponseParameters_Apis_ApiId",
                        column: x => x.ApiId,
                        principalTable: "Apis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResponseParameters_ObjectTypes_ObjectTypeId",
                        column: x => x.ObjectTypeId,
                        principalTable: "ObjectTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ObjectTypes_ApiId",
                table: "ObjectTypes",
                column: "ApiId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectTypes_ParentId",
                table: "ObjectTypes",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestParameters_ApiId",
                table: "RequestParameters",
                column: "ApiId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponseParameters_ApiId",
                table: "ResponseParameters",
                column: "ApiId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponseParameters_ObjectTypeId",
                table: "ResponseParameters",
                column: "ObjectTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestParameters");

            migrationBuilder.DropTable(
                name: "ResponseParameters");

            migrationBuilder.DropTable(
                name: "ObjectTypes");

            migrationBuilder.DropTable(
                name: "Apis");
        }
    }
}
