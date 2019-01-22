using Microsoft.EntityFrameworkCore.Migrations;

namespace ITour.Data.Migrations
{
    public partial class Remove_ManualProfit_from_Profits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManualProfit",
                table: "Profits");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ManualProfit",
                table: "Profits",
                type: "decimal(9, 2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
