using Microsoft.EntityFrameworkCore.Migrations;

namespace ITour.Data.Migrations
{
    public partial class Sequence_in_Service : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sequence",
                table: "Services",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "Services");
        }
    }
}
