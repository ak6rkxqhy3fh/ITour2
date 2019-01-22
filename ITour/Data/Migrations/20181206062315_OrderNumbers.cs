using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ITour.Data.Migrations
{
    public partial class OrderNumbers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TouroperatorCompanies_TouroperatorBrands_TouroperatorBrandId",
                table: "TouroperatorCompanies");

            migrationBuilder.DropIndex(
                name: "IX_TouroperatorCompanies_TouroperatorBrandId",
                table: "TouroperatorCompanies");

            migrationBuilder.DropColumn(
                name: "TouroperatorBrandId",
                table: "TouroperatorCompanies");

            migrationBuilder.CreateTable(
                name: "OrderNumbers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    Value = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderNumbers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderNumbers");

            migrationBuilder.AddColumn<Guid>(
                name: "TouroperatorBrandId",
                table: "TouroperatorCompanies",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TouroperatorCompanies_TouroperatorBrandId",
                table: "TouroperatorCompanies",
                column: "TouroperatorBrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_TouroperatorCompanies_TouroperatorBrands_TouroperatorBrandId",
                table: "TouroperatorCompanies",
                column: "TouroperatorBrandId",
                principalTable: "TouroperatorBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
