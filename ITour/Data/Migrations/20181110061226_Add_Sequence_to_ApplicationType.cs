using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ITour.Data.Migrations
{
    public partial class Add_Sequence_to_ApplicationType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("1a1d4bf3-2aea-41a3-bacd-93995073acc1"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("2875b5be-c2ac-4418-ba5f-db2418d591ed"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("3b7a174a-adb3-4aa2-b57f-276241eef31e"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("6a41907d-f228-47fc-b630-eb0753f09392"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("6fb3fcc5-b21a-41a5-a459-7018d238e04b"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("7060a7e3-a2f3-47ee-a2b3-e995f8e58a52"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("75c018d0-f0a5-4909-83b6-c1a8b0666898"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("9b17c975-ccec-4592-8f8a-59a3f1814e2e"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: new Guid("fd7c3b87-70ee-4b92-b2cd-dc384fb65807"));

            migrationBuilder.AddColumn<int>(
                name: "Sequence",
                table: "ServiceTypes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sequence",
                table: "AppTypes",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "ServiceTypes");

            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "AppTypes");

            migrationBuilder.InsertData(
                table: "ServiceTypes",
                columns: new[] { "Id", "Handler", "IsDeleted", "IsSystem", "Name", "TenantId" },
                values: new object[,]
                {
                    { new Guid("2875b5be-c2ac-4418-ba5f-db2418d591ed"), "PacketTourServices", false, true, "Пакетный тур", null },
                    { new Guid("6a41907d-f228-47fc-b630-eb0753f09392"), "TransportServices", false, true, "Транспорт", null },
                    { new Guid("fd7c3b87-70ee-4b92-b2cd-dc384fb65807"), "TransferServices", false, true, "Трансфер", null },
                    { new Guid("1a1d4bf3-2aea-41a3-bacd-93995073acc1"), "AccomodationServices", false, true, "Размещение", null },
                    { new Guid("7060a7e3-a2f3-47ee-a2b3-e995f8e58a52"), "InsuranceServices", false, true, "Страховка", null },
                    { new Guid("3b7a174a-adb3-4aa2-b57f-276241eef31e"), "VisaServices", false, true, "Виза", null },
                    { new Guid("9b17c975-ccec-4592-8f8a-59a3f1814e2e"), "ExcursionServices", false, true, "Экскурсия", null },
                    { new Guid("75c018d0-f0a5-4909-83b6-c1a8b0666898"), "FuelSurchargeServices", false, true, "Типливный сбор", null },
                    { new Guid("6fb3fcc5-b21a-41a5-a459-7018d238e04b"), "AdditionalServices", false, true, "Доп услуга", null }
                });
        }
    }
}
