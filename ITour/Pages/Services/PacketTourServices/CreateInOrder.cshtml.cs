using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using ITour.Data;
using ITour.Models;
using ITour.Services.Tenants;

namespace ITour.Pages.Services.PacketTourServices
{
    public class CreateInOrderModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ITenantProvider _tenantProvider;

        public CreateInOrderModel(ApplicationDbContext context, ITenantProvider tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }

        [BindProperty]
        public PacketTourService PacketTourService { get; set; }
        [BindProperty]
        public TransportService TransportServiceThere { get; set; }
        [BindProperty]
        public TransportService TransportServiceBack { get; set; }
        [BindProperty]
        public FuelSurchargeService FuelSurchargeService { get; set; }
        [BindProperty]
        public MaintenanceService MaintenanceService { get; set; }
        [BindProperty]
        public TransferService TransferService { get; set; }
        [BindProperty]
        public AccomodationService AccomodationService { get; set; }
        [BindProperty]
        public InsuranceService InsuranceServiceMed { get; set; }
        [BindProperty]
        public InsuranceService InsuranceServiceTrip { get; set; }
        [BindProperty]
        public VisaService VisaService { get; set; }
        [BindProperty]
        public ExcursionService ExcursionService { get; set; }

        [BindProperty]
        public Hotel Hotel { get; set; }

        public IActionResult OnGet()
        {
            ViewData["CountryId"] = new SelectList(_context.Countries.OrderBy(c => c.Name).AsNoTracking(), "Id", "Name", AccomodationService?.CountryId);

            IQueryable<Resort> resorts = _context.Resorts.OrderBy(r => r.Name).AsNoTracking();
            if (AccomodationService?.CountryId != null)
                resorts = resorts.Where(r => r.CountryId == AccomodationService.CountryId);
            ViewData["ResortId"] = new SelectList(resorts, "Id", "Name", AccomodationService?.ResortId);

            IQueryable<Hotel> hotels = _context.Hotels.OrderBy(h => h.Name).AsNoTracking();
            if (AccomodationService?.ResortId != null)
                hotels = hotels.Where(h => h.ResortId == AccomodationService.ResortId);
            ViewData["HotelId"] = new SelectList(hotels, "Id", "NameSelect", AccomodationService?.HotelId);

            ViewData["FoodTypeId"] = new SelectList(_context.FoodTypes.AsNoTracking(), "Id", "Name", AccomodationService?.FoodTypeId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes.AsNoTracking(), "Id", "Name", AccomodationService?.RoomTypeId);
            ViewData["CurrencyTypeId"] = new SelectList(_context.CurrencyTypes.AsNoTracking(), "Id", "Name", AccomodationService?.CurrencyTypeId);


            ViewData["TransportThereTypeId"] = new SelectList(_context.TransportTypes.AsNoTracking(), "Id", "Name", TransportServiceThere?.TransportTypeId);
            ViewData["TransportBackTypeId"] = new SelectList(_context.TransportTypes.AsNoTracking(), "Id", "Name", TransportServiceBack?.TransportTypeId);

            ViewData["TransferTypeId"] = new SelectList(_context.TransferTypes.AsNoTracking(), "Id", "Name", TransferService?.TransferTypeId);

            ViewData["InsuranceCompanyIdMed"] = new SelectList(_context.InsuranceCompanies.AsNoTracking(), "Id", "Name", InsuranceServiceMed?.InsuranceCompanyId);
            ViewData["InsuranceTypeIdMed"] = new SelectList(_context.InsuranceTypes.Where(it => it.Name == "Медицинская").AsNoTracking(), "Id", "Name");

            ViewData["InsuranceCompanyIdTrip"] = new SelectList(_context.InsuranceCompanies.AsNoTracking(), "Id", "Name", InsuranceServiceTrip?.InsuranceCompanyId);
            ViewData["InsuranceTypeIdTrip"] = new SelectList(_context.InsuranceTypes.Where(it => it.Name == "От невыезда").AsNoTracking(), "Id", "Name");

            ViewData["VisaTypeId"] = new SelectList(_context.VisaTypes.AsNoTracking(), "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            string returnPage = (string)TempData["ReturnPage"];
            Guid orderId = (Guid)TempData["OrderId"];
            Guid tenantId = _tenantProvider.Tenant.Id;

            PacketTourService.OrderId = orderId;
            PacketTourService.TenantId = tenantId;
            AccomodationService.OrderId = orderId;
            AccomodationService.TenantId = tenantId;
            TransportServiceThere.OrderId = orderId;
            TransportServiceThere.TenantId = tenantId;
            TransportServiceBack.OrderId = orderId;
            TransportServiceBack.TenantId = tenantId;
            FuelSurchargeService.OrderId = orderId;
            FuelSurchargeService.TenantId = tenantId;
            MaintenanceService.OrderId = orderId;
            MaintenanceService.TenantId = tenantId;
            TransferService.OrderId = orderId;
            TransferService.TenantId = tenantId;
            InsuranceServiceMed.OrderId = orderId;
            InsuranceServiceMed.TenantId = tenantId;
            InsuranceServiceTrip.OrderId = orderId;
            InsuranceServiceTrip.TenantId = tenantId;
            VisaService.OrderId = orderId;
            VisaService.TenantId = tenantId;
            ExcursionService.OrderId = orderId;
            ExcursionService.TenantId = tenantId;


            PacketTourService.Cost = PacketTourService.Cost ?? 0;

            PacketTourService.Services.Add(AccomodationService);
            PacketTourService.Services.Add(TransportServiceThere);
            PacketTourService.Services.Add(TransportServiceBack);
            PacketTourService.Services.Add(FuelSurchargeService);
            PacketTourService.Services.Add(MaintenanceService);
            PacketTourService.Services.Add(TransferService);
            PacketTourService.Services.Add(InsuranceServiceMed);
            PacketTourService.Services.Add(InsuranceServiceTrip);
            PacketTourService.Services.Add(VisaService);
            PacketTourService.Services.Add(ExcursionService);

            _context.PacketTourServices.Add(PacketTourService);

            await _context.SaveChangesAsync();

            Order order = await _context.Orders.IgnoreQueryFilters().FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null || order.TenantId != _tenantProvider.Tenant.Id || order.IsDeleted)
                return NotFound();

            order.DateBegin = TransportServiceThere.DateBegin;
            order.DateEnd = TransportServiceBack.DateEnd;

            if (TransportServiceThere.DateBegin.HasValue && TransportServiceBack.DateEnd.HasValue)
            {
                order.CountNights = ((DateTime)TransportServiceBack.DateEnd - (DateTime)TransportServiceThere.DateBegin).Days;
                order.CountDays = order.CountNights + 1;
            }
            _context.Attach(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToPage(returnPage, "", new { id = orderId }, "Services");
        }

        public JsonResult OnGetCascadingResorts(Guid countryId)
        {
            IQueryable<Resort> resorts = _context.Resorts.Where(r => r.CountryId == countryId).OrderBy(c => c.Name).AsNoTracking();
            return new JsonResult(new SelectList(resorts, "Id", "Name"));
        }

        public JsonResult OnGetCascadingHotels(Guid resortId)
        {
            IQueryable<Hotel> hotels = _context.Hotels.Where(r => r.ResortId == resortId).OrderBy(r => r.NameEn).AsNoTracking();
            return new JsonResult(new SelectList(hotels, "Id", "NameSelect"));
        }

        public JsonResult OnGetCreateHotel(string name, string nameEn, Guid resortId)
        {
            Hotel hotel = new Hotel()
            {
                Name = name,
                NameEn = nameEn,
                ResortId = resortId,
                TenantId = _tenantProvider.Tenant.Id
            };

            _context.Hotels.Add(hotel);
            _context.SaveChanges();

            IQueryable<Hotel> hotels = _context.Hotels.Where(h => h.ResortId == resortId).AsNoTracking();
            return new JsonResult(new SelectList(hotels, "Id", "NameSelect", hotel.Id));
        }
    }
}