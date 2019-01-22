using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Services.AccomodationServices.Hotels
{
    public class IndexGroupModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexGroupModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<HotelGroups> HotelGroups { get; set; }

        public void OnGet()
        {
            HotelGroups = _context.Hotels.Include(h => h.Resort).ThenInclude(r => r.Country)
                 .GroupBy(h => new { h.Resort.Country, h.Resort })
                 .OrderBy(g => g.Key.Country.Name).ThenBy(g => g.Key.Resort.Name)
                 .Select(g => new HotelGroups { Country = g.Key.Country, Resort = g.Key.Resort, Hotels = g.Select(h => h).ToList() })
                 .ToList();
        }
    }

    public class HotelGroups
    {
        public Country Country { get; set; }
        public Resort Resort { get; set; }
        public IEnumerable<Hotel> Hotels { get; set; }
    }
}
