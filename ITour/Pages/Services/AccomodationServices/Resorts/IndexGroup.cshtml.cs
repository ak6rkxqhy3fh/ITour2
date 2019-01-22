using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Services.AccomodationServices.Resorts
{
    public class IndexGroupModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexGroupModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<IGrouping<Country, Resort>> ResortGroups { get; set; }

        public async Task OnGetAsync()
        {
            ResortGroups = await _context.Resorts.Include(r => r.Country).OrderBy(r => r.Name)
                .GroupBy(r => r.Country).OrderBy(e => e.Key.Name)
                .AsNoTracking().ToListAsync();

        }
    }
}
