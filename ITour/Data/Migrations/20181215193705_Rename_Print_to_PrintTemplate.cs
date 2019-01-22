using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ITour.Data.Migrations
{
    public partial class Rename_Print_to_PrintTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prints");

            migrationBuilder.CreateTable(
                name: "PrintTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ContentTemplate = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Sequence = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrintTemplates", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrintTemplates");

            migrationBuilder.CreateTable(
                name: "Prints",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ContentTemplate = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Sequence = table.Column<int>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prints", x => x.Id);
                });
        }
    }
}
