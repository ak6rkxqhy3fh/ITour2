using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ITour.Data.Migrations
{
    public partial class TouroperatorBrandCompanies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TouroperatorBrandCompanies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    TouroperatorCompanyId = table.Column<Guid>(nullable: false),
                    TouroperatorBrandId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TouroperatorBrandCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TouroperatorBrandCompanies_TouroperatorBrands_TouroperatorBrandId",
                        column: x => x.TouroperatorBrandId,
                        principalTable: "TouroperatorBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TouroperatorBrandCompanies_TouroperatorCompanies_TouroperatorCompanyId",
                        column: x => x.TouroperatorCompanyId,
                        principalTable: "TouroperatorCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TouroperatorBrandCompanies_TouroperatorBrandId",
                table: "TouroperatorBrandCompanies",
                column: "TouroperatorBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_TouroperatorBrandCompanies_TouroperatorCompanyId",
                table: "TouroperatorBrandCompanies",
                column: "TouroperatorCompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TouroperatorBrandCompanies");
        }
    }
}
