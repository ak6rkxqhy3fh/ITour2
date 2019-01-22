using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Models;
using ITour.Data;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ITour.Utilities;

namespace ITour.Pages.Services.AccomodationServices.Hotels
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Hotel> Hotel { get; set; }
        [BindProperty(SupportsGet = true)]
        public HotelFilter HotelFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public HotelPaginate HotelPaginate { get; set; }

        public IList<Country> Countries { get; set; }

        public async Task OnGetAsync()
        {

            IQueryable<Hotel> hotelIQ = _context.Hotels
                .Include(h => h.Resort).ThenInclude(r => r.Country)
                .OrderBy(h => h.Resort.Country.Name).ThenBy(h => h.Resort.Name).ThenBy(h => h.Name);

            hotelIQ = HotelFilter.Process(hotelIQ);

            hotelIQ = HotelPaginate.Process(hotelIQ);

            Hotel = await hotelIQ.AsNoTracking().ToListAsync();

            ViewData["FilterCountryId"] = new SelectList(_context.Countries.OrderBy(c => c.Name).AsNoTracking(), "Id", "Name");
            IQueryable<Resort> resorts = _context.Resorts.OrderBy(c => c.Name).AsNoTracking();
            if  (HotelFilter?.CountryId != null)
                resorts = resorts.Where(r => r.CountryId == HotelFilter.CountryId);
            ViewData["FilterResortId"] = new SelectList(resorts, "Id", "Name");
            ViewData["PageSize"] = new SelectList(HotelPaginate.PageSizeDictionary, "Key", "Value", HotelPaginate.PageSize);
        }

        public JsonResult OnGetCascadingResorts(Guid countryId)
        {
            List<Resort> resortList = new List<Resort>();
            resortList = _context.Resorts.Where(r => r.CountryId == countryId).OrderBy(c => c.Name).AsNoTracking().ToList();
            return new JsonResult(new SelectList(resortList, "Id", "Name"));
        }
    }

    public class HotelFilter
    {
        [Display(Name = "Страна")]
        public Guid? CountryId { get; set; }
        public Guid? ResortId { get; set; }

        public bool NotAllParamsIsNull => CountryId != null || ResortId != null;

        public IQueryable<Hotel> Process(IQueryable<Hotel> hotelIQ)
        {
            if (CountryId != null)
                hotelIQ = hotelIQ.Where(r => r.Resort.CountryId == CountryId);

            if (ResortId != null)
                hotelIQ = hotelIQ.Where(r => r.ResortId == ResortId);

            return hotelIQ;
        }
    }

    public class HotelPaginate : Paginate<Hotel> { }
}
