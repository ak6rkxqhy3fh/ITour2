using Microsoft.EntityFrameworkCore.Migrations;

namespace ITour.Data.Migrations
{
    public partial class TouristsCountnullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TouristsCount",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TouristsCount",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
