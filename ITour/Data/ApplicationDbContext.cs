using System;
using ITour.Models;
using ITour.Services.Options;
using ITour.Models.RegistryTO;
using ITour.Services.Tenants;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ITour.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly ITenantProvider _tenantProvider;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        public DbSet<AppFile> AppFiles { get; set; }
        public DbSet<AppType> AppTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderNumber> OrderNumbers  { get; set; }
        public DbSet<OrderTouroperatorCompany> OrderTouroperatorCompanies { get; set; }
        public DbSet<OrderTourist> OrderTourists { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<PaymentForm> PaymentForms { get; set; }
        public DbSet<IncomingPayment> IncomingPayments { get; set; }
        public DbSet<OutgoingPayment> OutgoingPayments { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<PowerAttorney> PowerAttorneys { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<AgencyCompany> AgencyCompanies { get; set; }
        public DbSet<AgencyOffice> AgencyOffices { get; set; }
        public DbSet<License> Licenses { get; set; }
        public DbSet<CustomerCompany> CustomerCompanies { get; set; }
        public DbSet<InsuranceCompany> InsuranceCompanies { get; set; }
        public DbSet<PartnerCompany> PartnerCompanies { get; set; }
        public DbSet<TouroperatorCompany> TouroperatorCompanies { get; set; }
        public DbSet<TouroperatorBrand> TouroperatorBrands { get; set; }
        public DbSet<TouroperatorBrandCompany> TouroperatorBrandCompanies { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<CurrencyType> CurrencyTypes { get; set; }
        public DbSet<PacketTourService> PacketTourServices { get; set; }
        public DbSet<AccomodationService> AccomodationServices { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Resort> Resorts { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<FoodType> FoodTypes { get; set; }
        public DbSet<TransportService> TransportServices { get; set; }
        public DbSet<TransportType> TransportTypes { get; set; }
        public DbSet<FuelSurchargeService> FuelSurchargeServices { get; set; }
        public DbSet<MaintenanceService> MaintenanceServices { get; set; }
        public DbSet<TransferService> TransferServices { get; set; }
        public DbSet<TransferType> TransferTypes { get; set; }
        public DbSet<InsuranceService> InsuranceServices { get; set; }
        public DbSet<InsuranceType> InsuranceTypes { get; set; }
        public DbSet<VisaService> VisaServices { get; set; }
        public DbSet<VisaType> VisaTypes { get; set; }
        public DbSet<ExcursionService> ExcursionServices { get; set; }
        public DbSet<AdditionalService> AdditionalServices { get; set; }

        public DbSet<CustomerFile> CustomerFiles { get; set; }
        public DbSet<PersonFile> PersonFile { get; set; }

        public DbSet<RegistryUri> RegistryUris { get; set; }

        public DbQuery<Commission> Commission { get; set; }

        public DbSet<Profit> Profits { get; set; }

        public DbSet<PrintTemplate> PrintTemplates { get; set; }

        public DbSet<AppOptions> AppOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ServiceType>().HasData(
            new ServiceType { Id = Guid.Parse("d4235cad-0794-4d14-b78e-c44c9e0d3a4a"), Name = "Пакетный тур",   Sequence = 1, Handler = "PacketTourServices" },
            new ServiceType { Id = Guid.Parse("20973be3-4bc3-4cb5-b29d-5b04ddbdd8eb"), Name = "Транспорт",      Sequence = 2, Handler = "TransportServices" },
            new ServiceType { Id = Guid.Parse("81e1c49e-3cae-481f-8211-330047a40e53"), Name = "Трансфер",       Sequence = 3, Handler = "TransferServices" },
            new ServiceType { Id = Guid.Parse("5e5fd872-3d6c-40b6-aaef-c3f6742f795b"), Name = "Размещение",     Sequence = 4, Handler = "AccomodationServices" },
            new ServiceType { Id = Guid.Parse("dfcc799a-760a-4918-9165-8967fcff6170"), Name = "Страховка",      Sequence = 5, Handler = "InsuranceServices" },
            new ServiceType { Id = Guid.Parse("562d76a3-0acf-4801-8562-958043fd1464"), Name = "Виза",           Sequence = 6, Handler = "VisaServices" },
            new ServiceType { Id = Guid.Parse("14e89c6f-ee6b-4b0c-ab59-cc097773f5d4"), Name = "Экскурсия",      Sequence = 7, Handler = "ExcursionServices" },
            new ServiceType { Id = Guid.Parse("9fd7b8c1-8235-4502-b49c-bae6b53631bd"), Name = "Типливный сбор", Sequence = 8, Handler = "FuelSurchargeServices" },
            new ServiceType { Id = Guid.Parse("49cee274-009f-43bc-b88d-a968f61f1fd3"), Name = "Доп услуга",     Sequence = 9, Handler = "AdditionalServices" }
            );

            modelBuilder.HasSequence<int>("OrderNumber").StartsAt(1).IncrementsBy(1);
            //modelBuilder.Entity<Order>().Property(o => o.Number).HasDefaultValueSql("NEXT VALUE FOR OrderNumber");


            modelBuilder.Entity<ApplicationUser>().HasQueryFilter(e => !e.IsDeleted && e.TenantId == _tenantProvider.Tenant.Id);

            modelBuilder.Entity<AppFile>().HasQueryFilter(e => !e.IsDeleted && e.TenantId == _tenantProvider.Tenant.Id);
            modelBuilder.Entity<AppType>().HasQueryFilter(e => !e.IsDeleted && e.TenantId == _tenantProvider.Tenant.Id);
            modelBuilder.Entity<AppOptions>().HasQueryFilter(e => e.TenantId == _tenantProvider.Tenant.Id);
            modelBuilder.Entity<PrintTemplate>().HasQueryFilter(e => !e.IsDeleted && e.TenantId == _tenantProvider.Tenant.Id);

            modelBuilder.Entity<Order>().HasQueryFilter(e => !e.IsDeleted && e.TenantId == _tenantProvider.Tenant.Id && e.Number != null);
            modelBuilder.Entity<OrderNumber>().HasQueryFilter(e => e.TenantId == _tenantProvider.Tenant.Id);
            modelBuilder.Entity<Person>().HasQueryFilter(e => !e.IsDeleted && e.TenantId == _tenantProvider.Tenant.Id);
            modelBuilder.Entity<Manager>().HasQueryFilter(e => !e.IsDeleted && e.TenantId == _tenantProvider.Tenant.Id);
            modelBuilder.Entity<PowerAttorney>().HasQueryFilter(e => !e.IsDeleted && e.TenantId == _tenantProvider.Tenant.Id);
            modelBuilder.Entity<Customer>().HasQueryFilter(e => !e.IsDeleted && e.TenantId == _tenantProvider.Tenant.Id);
            modelBuilder.Entity<Company>().HasQueryFilter(e => !e.IsDeleted && e.TenantId == _tenantProvider.Tenant.Id);
            modelBuilder.Entity<License>().HasQueryFilter(e => !e.IsDeleted && e.TenantId == _tenantProvider.Tenant.Id);

            modelBuilder.Entity<TouroperatorCompany>().HasQueryFilter(e => !e.IsDeleted && (e.TenantId == _tenantProvider.Tenant.Id || e.TenantId == null));

            modelBuilder.Entity<TouroperatorBrand>().HasQueryFilter(e => !e.IsDeleted && e.TenantId == _tenantProvider.Tenant.Id);
            modelBuilder.Entity<TouroperatorBrandCompany>().HasQueryFilter(e => e.TenantId == _tenantProvider.Tenant.Id);
            modelBuilder.Entity<Country>().HasQueryFilter(e => !e.IsDeleted && e.TenantId == _tenantProvider.Tenant.Id);
            modelBuilder.Entity<Resort>().HasQueryFilter(e => !e.IsDeleted && e.TenantId == _tenantProvider.Tenant.Id);
            modelBuilder.Entity<Hotel>().HasQueryFilter(e => !e.IsDeleted && e.TenantId == _tenantProvider.Tenant.Id);

            modelBuilder.Entity<OrderTouroperatorCompany>().HasQueryFilter(e => e.TenantId == _tenantProvider.Tenant.Id);
            modelBuilder.Entity<OrderTourist>().HasQueryFilter(e => e.TenantId == _tenantProvider.Tenant.Id);
            modelBuilder.Entity<Service>().HasQueryFilter(e => e.TenantId == _tenantProvider.Tenant.Id);
            modelBuilder.Entity<IncomingPayment>().HasQueryFilter(e => e.TenantId == _tenantProvider.Tenant.Id);
            modelBuilder.Entity<OutgoingPayment>().HasQueryFilter(e => e.TenantId == _tenantProvider.Tenant.Id);

            modelBuilder.Query<Commission>().HasQueryFilter(e => !e.IsDeleted && e.TenantId == _tenantProvider.Tenant.Id && e.OrderNumber != null);

        }
    }
}
