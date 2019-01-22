using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.AppCompanies.Companies.AgencyCompanies.Licenses
{
    public class IndexModel : PageModel
    {
        private readonly ITour.Data.ApplicationDbContext _context;

        public IndexModel(ITour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<License> License { get;set; }

        public async Task OnGetAsync()
        {
            License = await _context.Licenses
                .Include(l => l.AgencyCompany)
                .Include(l => l.AgencyOffice).ToListAsync();
        }
    }
}
