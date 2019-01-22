using Microsoft.EntityFrameworkCore.Migrations;

namespace ITour.Data.Migrations
{
    public partial class Add_IsActiveSequence_to_Print : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Prints",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Sequence",
                table: "Prints",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Prints");

            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "Prints");
        }
    }
}
