using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ITour.Data;
using ITour.Models;
using ITour.Services.Tenants;

namespace ITour.Pages.Services.AccomodationServices
{
    public class EditInOrderModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ITenantProvider _tenantProvider;

        public EditInOrderModel(ApplicationDbContext context, ITenantProvider tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }

        [BindProperty]
        public AccomodationService AccomodationService { get; set; }

        [BindProperty]
        public Hotel Hotel { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
                return NotFound();

                AccomodationService = await _context.AccomodationServices.FirstOrDefaultAsync(m => m.Id == id);

            if (AccomodationService == null)
                return NotFound();
            
            PopulateSelectLists(AccomodationService);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            AccomodationService.Cost = AccomodationService.Cost ?? 0;
            _context.Attach(AccomodationService).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccomodationServiceExists(AccomodationService.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            string returnPage = (string)TempData["ReturnPage"];
            Guid orderId = (Guid)TempData["OrderId"];

            return RedirectToPage(returnPage, "", new { id = orderId }, "Services");
        }

        private bool AccomodationServiceExists(Guid id)
        {
            return _context.AccomodationServices.Any(e => e.Id == id);
        }

        public JsonResult OnGetCascadingResorts(Guid countryId)
        {
            List<Resort> resortList = new List<Resort>();
            resortList = _context.Resorts.Where(r => r.CountryId == countryId).AsNoTracking().ToList();
            return new JsonResult(new SelectList(resortList, "Id", "Name"));
        }

        public JsonResult OnGetCascadingHotels(Guid resortId)
        {
            List<Hotel> hotelList = new List<Hotel>();
            hotelList = _context.Hotels.Where(r => r.ResortId == resortId).AsNoTracking().ToList();
            return new JsonResult(new SelectList(hotelList, "Id", "NameSelect"));
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

        public void PopulateSelectLists(AccomodationService accomodationService = null)
        {
            ViewData["CountryId"] = new SelectList(_context.Countries.OrderBy(c => c.Name).AsNoTracking(), "Id", "Name", accomodationService?.CountryId);

            IQueryable<Resort> resorts = _context.Resorts.OrderBy(r => r.Name).AsNoTracking();
            if (accomodationService?.CountryId != null)
                resorts = resorts.Where(r => r.CountryId == accomodationService.CountryId);
            ViewData["ResortId"] = new SelectList(resorts, "Id", "Name", accomodationService?.ResortId);

            IQueryable<Hotel> hotels = _context.Hotels.OrderBy(h => h.Name).AsNoTracking();
            if (accomodationService?.ResortId != null)
                hotels = hotels.Where(h => h.ResortId == accomodationService.ResortId);
            ViewData["HotelId"] = new SelectList(hotels, "Id", "NameSelect", accomodationService?.HotelId);

            ViewData["FoodTypeId"] = new SelectList(_context.FoodTypes.AsNoTracking(), "Id", "Name", accomodationService?.FoodTypeId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes.AsNoTracking(), "Id", "Name", accomodationService?.RoomTypeId);
            ViewData["CurrencyTypeId"] = new SelectList(_context.CurrencyTypes.AsNoTracking(), "Id", "Name", accomodationService?.CurrencyTypeId);
        }
    }
}
