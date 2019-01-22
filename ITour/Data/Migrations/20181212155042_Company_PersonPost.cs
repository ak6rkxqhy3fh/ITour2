using Microsoft.EntityFrameworkCore.Migrations;

namespace ITour.Data.Migrations
{
    public partial class Company_PersonPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PersonPost",
                table: "Companies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonPost",
                table: "Companies");
        }
    }
}
