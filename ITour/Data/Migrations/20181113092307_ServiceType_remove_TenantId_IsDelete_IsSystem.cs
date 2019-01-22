using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ITour.Data.Migrations
{
    public partial class ServiceType_remove_TenantId_IsDelete_IsSystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ServiceTypes");

            migrationBuilder.DropColumn(
                name: "IsSystem",
                table: "ServiceTypes");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ServiceTypes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ServiceTypes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSystem",
                table: "ServiceTypes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "ServiceTypes",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("14e89c6f-ee6b-4b0c-ab59-cc097773f5d4"),
                column: "IsSystem",
                value: true);

            migrationBuilder.UpdateData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("20973be3-4bc3-4cb5-b29d-5b04ddbdd8eb"),
                column: "IsSystem",
                value: true);

            migrationBuilder.UpdateData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("49cee274-009f-43bc-b88d-a968f61f1fd3"),
                column: "IsSystem",
                value: true);

            migrationBuilder.UpdateData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("562d76a3-0acf-4801-8562-958043fd1464"),
                column: "IsSystem",
                value: true);

            migrationBuilder.UpdateData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("5e5fd872-3d6c-40b6-aaef-c3f6742f795b"),
                column: "IsSystem",
                value: true);

            migrationBuilder.UpdateData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("81e1c49e-3cae-481f-8211-330047a40e53"),
                column: "IsSystem",
                value: true);

            migrationBuilder.UpdateData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("9fd7b8c1-8235-4502-b49c-bae6b53631bd"),
                column: "IsSystem",
                value: true);

            migrationBuilder.UpdateData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("d4235cad-0794-4d14-b78e-c44c9e0d3a4a"),
                column: "IsSystem",
                value: true);

            migrationBuilder.UpdateData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("dfcc799a-760a-4918-9165-8967fcff6170"),
                column: "IsSystem",
                value: true);
        }
    }
}
