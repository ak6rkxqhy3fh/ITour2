using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ITour.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "OrderNumber");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.CreateTable(
                name: "AppTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsSystem = table.Column<bool>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistryUris",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UriString = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistryUris", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsSystem = table.Column<bool>(nullable: false),
                    Handler = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TouroperatorBrands",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TouroperatorBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    Firstname = table.Column<string>(nullable: true),
                    Middlename = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    SurnameGenitive = table.Column<string>(nullable: true),
                    FirstnameGenitive = table.Column<string>(nullable: true),
                    MiddlenameGenitive = table.Column<string>(nullable: true),
                    IdDocument_DocumentTypeId = table.Column<Guid>(nullable: true),
                    IdDocument_Series = table.Column<string>(nullable: true),
                    IdDocument_Number = table.Column<string>(nullable: true),
                    IdDocument_IssuedBy = table.Column<string>(nullable: true),
                    IdDocument_IssuedDate = table.Column<DateTime>(nullable: true),
                    IdDocument_ExpirationDate = table.Column<DateTime>(nullable: true),
                    Passport_DocumentTypeId = table.Column<Guid>(nullable: true),
                    Passport_Series = table.Column<string>(nullable: true),
                    Passport_Number = table.Column<string>(nullable: true),
                    Passport_IssuedBy = table.Column<string>(nullable: true),
                    Passport_IssuedDate = table.Column<DateTime>(nullable: true),
                    Passport_ExpirationDate = table.Column<DateTime>(nullable: true),
                    IsEmployee = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                    table.ForeignKey(
                        name: "FK_People_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_People_AppTypes_IdDocument_DocumentTypeId",
                        column: x => x.IdDocument_DocumentTypeId,
                        principalTable: "AppTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_People_AppTypes_Passport_DocumentTypeId",
                        column: x => x.Passport_DocumentTypeId,
                        principalTable: "AppTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Resorts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CountryId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resorts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resorts_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TouroperatorCompanies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    INN = table.Column<string>(nullable: true),
                    OGRN = table.Column<string>(nullable: true),
                    AddressLegal = table.Column<string>(nullable: true),
                    AddressPostal = table.Column<string>(nullable: true),
                    Bank = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    TouroperatorBrandId = table.Column<Guid>(nullable: true),
                    RegistryNumber = table.Column<string>(nullable: true),
                    FinGaranteeTotalAmount = table.Column<string>(nullable: true),
                    FinGaranteeAmountNewPeriod = table.Column<string>(nullable: true),
                    FinGaranteeExpirationDateNewPeriod = table.Column<string>(nullable: true),
                    JsonData = table.Column<string>(nullable: true),
                    IsValid = table.Column<bool>(nullable: false),
                    IsOpenData = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TouroperatorCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TouroperatorCompanies_TouroperatorBrands_TouroperatorBrandId",
                        column: x => x.TouroperatorBrandId,
                        principalTable: "TouroperatorBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    INN = table.Column<string>(nullable: true),
                    OGRN = table.Column<string>(nullable: true),
                    AddressLegal = table.Column<string>(nullable: true),
                    AddressPostal = table.Column<string>(nullable: true),
                    Bank = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    PersonId = table.Column<Guid>(nullable: true),
                    ActionCause = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Hotels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ResortId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NameEn = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hotels_Resorts_ResortId",
                        column: x => x.ResortId,
                        principalTable: "Resorts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Licenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    AgencyCompanyId = table.Column<Guid>(nullable: true),
                    AgencyOfficeId = table.Column<Guid>(nullable: true),
                    LicenseNumber = table.Column<string>(nullable: true),
                    LicenseDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Licenses_Companies_AgencyCompanyId",
                        column: x => x.AgencyCompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Licenses_Companies_AgencyOfficeId",
                        column: x => x.AgencyOfficeId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PersonId = table.Column<Guid>(nullable: true),
                    AgencyOfficeId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Managers_Companies_AgencyOfficeId",
                        column: x => x.AgencyOfficeId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Managers_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PersonId = table.Column<Guid>(nullable: true),
                    ManagerId = table.Column<Guid>(nullable: true),
                    CustomerCompanyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Companies_CustomerCompanyId",
                        column: x => x.CustomerCompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customers_Managers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Managers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customers_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PowerAttorneys",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    IssuedDate = table.Column<DateTime>(nullable: true),
                    ManagerId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerAttorneys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PowerAttorneys_Managers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Managers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: true),
                    PersonId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppFiles_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppFiles_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Number = table.Column<int>(nullable: true, defaultValueSql: "NEXT VALUE FOR OrderNumber"),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    DatePrint = table.Column<DateTime>(nullable: true),
                    DateBegin = table.Column<DateTime>(nullable: true),
                    DateEnd = table.Column<DateTime>(nullable: true),
                    CountDays = table.Column<int>(nullable: true),
                    CountNights = table.Column<int>(nullable: true),
                    OrderStatusId = table.Column<Guid>(nullable: true),
                    ReservationNumber = table.Column<string>(nullable: true),
                    AgencyCompanyId = table.Column<Guid>(nullable: true),
                    ManagerId = table.Column<Guid>(nullable: true),
                    CustomerId = table.Column<Guid>(nullable: true),
                    TouroperatorBrandId = table.Column<Guid>(nullable: true),
                    TouristsString = table.Column<string>(nullable: true),
                    TouristsCount = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    AgencyOfficeId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Companies_AgencyCompanyId",
                        column: x => x.AgencyCompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Companies_AgencyOfficeId",
                        column: x => x.AgencyOfficeId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Managers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Managers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_AppTypes_OrderStatusId",
                        column: x => x.OrderStatusId,
                        principalTable: "AppTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_TouroperatorBrands_TouroperatorBrandId",
                        column: x => x.TouroperatorBrandId,
                        principalTable: "TouroperatorBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderTourists",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrderId = table.Column<Guid>(nullable: false),
                    PersonId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTourists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderTourists_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderTourists_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderTouroperatorCompanies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrderId = table.Column<Guid>(nullable: false),
                    TouroperatorCompanyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTouroperatorCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderTouroperatorCompanies_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderTouroperatorCompanies_TouroperatorCompanies_TouroperatorCompanyId",
                        column: x => x.TouroperatorCompanyId,
                        principalTable: "TouroperatorCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentsIncoming",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrderId = table.Column<Guid>(nullable: false),
                    PaymentTypeId = table.Column<Guid>(nullable: true),
                    PaymentFormId = table.Column<Guid>(nullable: true),
                    PaymentDate = table.Column<DateTime>(nullable: true),
                    PaymentAmount = table.Column<decimal>(type: "decimal(9, 2)", nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    ReceiptNumber = table.Column<string>(nullable: true),
                    BankCommission = table.Column<decimal>(type: "decimal(9, 2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentsIncoming", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentsIncoming_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentsIncoming_AppTypes_PaymentFormId",
                        column: x => x.PaymentFormId,
                        principalTable: "AppTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentsIncoming_AppTypes_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "AppTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentsOutgoing",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrderId = table.Column<Guid>(nullable: false),
                    PaymentTypeId = table.Column<Guid>(nullable: true),
                    PaymentFormId = table.Column<Guid>(nullable: true),
                    PaymentDate = table.Column<DateTime>(nullable: true),
                    PaymentAmount = table.Column<decimal>(type: "decimal(9, 2)", nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    InvoiceNumber = table.Column<string>(nullable: true),
                    InvoiceDate = table.Column<DateTime>(nullable: true),
                    PartnerCompanyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentsOutgoing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentsOutgoing_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentsOutgoing_Companies_PartnerCompanyId",
                        column: x => x.PartnerCompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentsOutgoing_AppTypes_PaymentFormId",
                        column: x => x.PaymentFormId,
                        principalTable: "AppTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentsOutgoing_AppTypes_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "AppTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrderId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    DateBegin = table.Column<DateTime>(nullable: true),
                    TimeBegin = table.Column<TimeSpan>(nullable: true),
                    DateEnd = table.Column<DateTime>(nullable: true),
                    TimeEnd = table.Column<TimeSpan>(nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(9, 2)", nullable: true),
                    CostPrice = table.Column<decimal>(type: "decimal(9, 2)", nullable: true),
                    CurrencyTypeId = table.Column<Guid>(nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "decimal(9, 2)", nullable: true),
                    PacketTourServiceId = table.Column<Guid>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    CountryId = table.Column<Guid>(nullable: true),
                    ResortId = table.Column<Guid>(nullable: true),
                    HotelId = table.Column<Guid>(nullable: true),
                    FoodTypeId = table.Column<Guid>(nullable: true),
                    FoodTypeString = table.Column<string>(nullable: true),
                    RoomTypeId = table.Column<Guid>(nullable: true),
                    RoomTypeString = table.Column<string>(nullable: true),
                    IsFuelSurcharge = table.Column<bool>(nullable: true),
                    InsuranceTypeId = table.Column<Guid>(nullable: true),
                    InsuranceCompanyId = table.Column<Guid>(nullable: true),
                    TransferTypeId = table.Column<Guid>(nullable: true),
                    TransportTypeId = table.Column<Guid>(nullable: true),
                    TripNumber = table.Column<string>(nullable: true),
                    DepartureSity = table.Column<string>(nullable: true),
                    DepartureTerminal = table.Column<string>(nullable: true),
                    ArrivalSity = table.Column<string>(nullable: true),
                    ArrivalTerminal = table.Column<string>(nullable: true),
                    Tickets = table.Column<string>(nullable: true),
                    VisaTypeId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Services_AppTypes_FoodTypeId",
                        column: x => x.FoodTypeId,
                        principalTable: "AppTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Services_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Services_Resorts_ResortId",
                        column: x => x.ResortId,
                        principalTable: "Resorts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Services_AppTypes_RoomTypeId",
                        column: x => x.RoomTypeId,
                        principalTable: "AppTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Services_Companies_InsuranceCompanyId",
                        column: x => x.InsuranceCompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Services_AppTypes_InsuranceTypeId",
                        column: x => x.InsuranceTypeId,
                        principalTable: "AppTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Services_AppTypes_CurrencyTypeId",
                        column: x => x.CurrencyTypeId,
                        principalTable: "AppTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Services_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Services_Services_PacketTourServiceId",
                        column: x => x.PacketTourServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Services_AppTypes_TransferTypeId",
                        column: x => x.TransferTypeId,
                        principalTable: "AppTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Services_AppTypes_TransportTypeId",
                        column: x => x.TransportTypeId,
                        principalTable: "AppTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Services_AppTypes_VisaTypeId",
                        column: x => x.VisaTypeId,
                        principalTable: "AppTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_AppFiles_CustomerId",
                table: "AppFiles",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppFiles_PersonId",
                table: "AppFiles",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_PersonId",
                table: "Companies",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerCompanyId",
                table: "Customers",
                column: "CustomerCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ManagerId",
                table: "Customers",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PersonId",
                table: "Customers",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_ResortId",
                table: "Hotels",
                column: "ResortId");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_AgencyCompanyId",
                table: "Licenses",
                column: "AgencyCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_AgencyOfficeId",
                table: "Licenses",
                column: "AgencyOfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_AgencyOfficeId",
                table: "Managers",
                column: "AgencyOfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_PersonId",
                table: "Managers",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AgencyCompanyId",
                table: "Orders",
                column: "AgencyCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AgencyOfficeId",
                table: "Orders",
                column: "AgencyOfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ManagerId",
                table: "Orders",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderStatusId",
                table: "Orders",
                column: "OrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TouroperatorBrandId",
                table: "Orders",
                column: "TouroperatorBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTourists_OrderId",
                table: "OrderTourists",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTourists_PersonId",
                table: "OrderTourists",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTouroperatorCompanies_OrderId",
                table: "OrderTouroperatorCompanies",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTouroperatorCompanies_TouroperatorCompanyId",
                table: "OrderTouroperatorCompanies",
                column: "TouroperatorCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsIncoming_OrderId",
                table: "PaymentsIncoming",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsIncoming_PaymentFormId",
                table: "PaymentsIncoming",
                column: "PaymentFormId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsIncoming_PaymentTypeId",
                table: "PaymentsIncoming",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsOutgoing_OrderId",
                table: "PaymentsOutgoing",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsOutgoing_PartnerCompanyId",
                table: "PaymentsOutgoing",
                column: "PartnerCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsOutgoing_PaymentFormId",
                table: "PaymentsOutgoing",
                column: "PaymentFormId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsOutgoing_PaymentTypeId",
                table: "PaymentsOutgoing",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_People_ApplicationUserId",
                table: "People",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_People_IdDocument_DocumentTypeId",
                table: "People",
                column: "IdDocument_DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_People_Passport_DocumentTypeId",
                table: "People",
                column: "Passport_DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PowerAttorneys_ManagerId",
                table: "PowerAttorneys",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Resorts_CountryId",
                table: "Resorts",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_CountryId",
                table: "Services",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_FoodTypeId",
                table: "Services",
                column: "FoodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_HotelId",
                table: "Services",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ResortId",
                table: "Services",
                column: "ResortId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_RoomTypeId",
                table: "Services",
                column: "RoomTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_InsuranceCompanyId",
                table: "Services",
                column: "InsuranceCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_InsuranceTypeId",
                table: "Services",
                column: "InsuranceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_CurrencyTypeId",
                table: "Services",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_OrderId",
                table: "Services",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_PacketTourServiceId",
                table: "Services",
                column: "PacketTourServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_TransferTypeId",
                table: "Services",
                column: "TransferTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_TransportTypeId",
                table: "Services",
                column: "TransportTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_VisaTypeId",
                table: "Services",
                column: "VisaTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TouroperatorCompanies_TouroperatorBrandId",
                table: "TouroperatorCompanies",
                column: "TouroperatorBrandId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppFiles");

            migrationBuilder.DropTable(
                name: "Licenses");

            migrationBuilder.DropTable(
                name: "OrderTourists");

            migrationBuilder.DropTable(
                name: "OrderTouroperatorCompanies");

            migrationBuilder.DropTable(
                name: "PaymentsIncoming");

            migrationBuilder.DropTable(
                name: "PaymentsOutgoing");

            migrationBuilder.DropTable(
                name: "PowerAttorneys");

            migrationBuilder.DropTable(
                name: "RegistryUris");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "ServiceTypes");

            migrationBuilder.DropTable(
                name: "TouroperatorCompanies");

            migrationBuilder.DropTable(
                name: "Hotels");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Resorts");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "TouroperatorBrands");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "AppTypes");

            migrationBuilder.DropSequence(
                name: "OrderNumber");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
