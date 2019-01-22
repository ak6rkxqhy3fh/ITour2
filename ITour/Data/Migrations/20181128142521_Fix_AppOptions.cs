using Microsoft.EntityFrameworkCore.Migrations;

namespace ITour.Data.Migrations
{
    public partial class Fix_AppOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsUseProfit",
                table: "AppOptions",
                newName: "IsUseProfits");

            migrationBuilder.RenameColumn(
                name: "IsUseCommission",
                table: "AppOptions",
                newName: "IsUsePayments");

            migrationBuilder.AddColumn<bool>(
                name: "IsUseCommissionReport",
                table: "AppOptions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUseCommissionReport",
                table: "AppOptions");

            migrationBuilder.RenameColumn(
                name: "IsUseProfits",
                table: "AppOptions",
                newName: "IsUseProfit");

            migrationBuilder.RenameColumn(
                name: "IsUsePayments",
                table: "AppOptions",
                newName: "IsUseCommission");
        }
    }
}
