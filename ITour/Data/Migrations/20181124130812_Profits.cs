using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ITour.Data.Migrations
{
    public partial class Profits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Profits",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    OrderId = table.Column<Guid>(nullable: false),
                    ManualBasicProfit = table.Column<decimal>(type: "decimal(9, 2)", nullable: false),
                    ManualAddProfit = table.Column<decimal>(type: "decimal(9, 2)", nullable: false),
                    ManualProfit = table.Column<decimal>(type: "decimal(9, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profits_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Profits_OrderId",
                table: "Profits",
                column: "OrderId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Profits");
        }
    }
}
