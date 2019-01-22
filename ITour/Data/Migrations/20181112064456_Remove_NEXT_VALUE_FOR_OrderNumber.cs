using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ITour.Data.Migrations
{
    public partial class Remove_NEXT_VALUE_FOR_OrderNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("17a697e8-2193-44ee-a953-783dcf98e4b3"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("54ccc3a1-0327-44ea-a9db-269f8bffa656"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("6f0ecbc6-ccff-49d0-8b84-5791d4819604"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("a3fbda79-56be-4b96-a138-fb09c69bce99"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("a616a1ae-f7c8-4e05-afec-be9570debb27"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("accf276f-3830-4b06-9393-3064c2d31145"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("c23065c6-aa8d-4c04-a6c2-378b5a57ab2e"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("f6ae2fba-4238-467b-a7e5-50f72811cf8a"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("ffb490de-c8f8-42f2-9c41-89f69e1d1046"));

            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true,
                oldDefaultValueSql: "NEXT VALUE FOR OrderNumber");

            migrationBuilder.InsertData(
                table: "ServiceTypes",
                columns: new[] { "Id", "Handler", "IsDeleted", "IsSystem", "Name", "Sequence", "TenantId" },
                values: new object[,]
                {
                    { new Guid("d4235cad-0794-4d14-b78e-c44c9e0d3a4a"), "PacketTourServices", false, true, "Пакетный тур", 1, null },
                    { new Guid("20973be3-4bc3-4cb5-b29d-5b04ddbdd8eb"), "TransportServices", false, true, "Транспорт", 2, null },
                    { new Guid("81e1c49e-3cae-481f-8211-330047a40e53"), "TransferServices", false, true, "Трансфер", 3, null },
                    { new Guid("5e5fd872-3d6c-40b6-aaef-c3f6742f795b"), "AccomodationServices", false, true, "Размещение", 4, null },
                    { new Guid("dfcc799a-760a-4918-9165-8967fcff6170"), "InsuranceServices", false, true, "Страховка", 5, null },
                    { new Guid("562d76a3-0acf-4801-8562-958043fd1464"), "VisaServices", false, true, "Виза", 6, null },
                    { new Guid("14e89c6f-ee6b-4b0c-ab59-cc097773f5d4"), "ExcursionServices", false, true, "Экскурсия", 7, null },
                    { new Guid("9fd7b8c1-8235-4502-b49c-bae6b53631bd"), "FuelSurchargeServices", false, true, "Типливный сбор", 8, null },
                    { new Guid("49cee274-009f-43bc-b88d-a968f61f1fd3"), "AdditionalServices", false, true, "Доп услуга", 9, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("14e89c6f-ee6b-4b0c-ab59-cc097773f5d4"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("20973be3-4bc3-4cb5-b29d-5b04ddbdd8eb"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("49cee274-009f-43bc-b88d-a968f61f1fd3"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("562d76a3-0acf-4801-8562-958043fd1464"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("5e5fd872-3d6c-40b6-aaef-c3f6742f795b"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("81e1c49e-3cae-481f-8211-330047a40e53"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("9fd7b8c1-8235-4502-b49c-bae6b53631bd"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("d4235cad-0794-4d14-b78e-c44c9e0d3a4a"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("dfcc799a-760a-4918-9165-8967fcff6170"));

            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "Orders",
                nullable: true,
                defaultValueSql: "NEXT VALUE FOR OrderNumber",
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "ServiceTypes",
                columns: new[] { "Id", "Handler", "IsDeleted", "IsSystem", "Name", "Sequence", "TenantId" },
                values: new object[,]
                {
                    { new Guid("f6ae2fba-4238-467b-a7e5-50f72811cf8a"), "PacketTourServices", false, true, "Пакетный тур", 1, null },
                    { new Guid("ffb490de-c8f8-42f2-9c41-89f69e1d1046"), "TransportServices", false, true, "Транспорт", 2, null },
                    { new Guid("6f0ecbc6-ccff-49d0-8b84-5791d4819604"), "TransferServices", false, true, "Трансфер", 3, null },
                    { new Guid("54ccc3a1-0327-44ea-a9db-269f8bffa656"), "AccomodationServices", false, true, "Размещение", 4, null },
                    { new Guid("17a697e8-2193-44ee-a953-783dcf98e4b3"), "InsuranceServices", false, true, "Страховка", 5, null },
                    { new Guid("c23065c6-aa8d-4c04-a6c2-378b5a57ab2e"), "VisaServices", false, true, "Виза", 6, null },
                    { new Guid("accf276f-3830-4b06-9393-3064c2d31145"), "ExcursionServices", false, true, "Экскурсия", 7, null },
                    { new Guid("a3fbda79-56be-4b96-a138-fb09c69bce99"), "FuelSurchargeServices", false, true, "Типливный сбор", 8, null },
                    { new Guid("a616a1ae-f7c8-4e05-afec-be9570debb27"), "AdditionalServices", false, true, "Доп услуга", 9, null }
                });
        }
    }
}
