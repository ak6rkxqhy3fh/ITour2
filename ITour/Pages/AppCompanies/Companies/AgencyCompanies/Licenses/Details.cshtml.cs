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
    public class DetailsModel : PageModel
    {
        private readonly ITour.Data.ApplicationDbContext _context;

        public DetailsModel(ITour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public License License { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            License = await _context.Licenses
                .Include(l => l.AgencyCompany)
                .Include(l => l.AgencyOffice).FirstOrDefaultAsync(m => m.Id == id);

            if (License == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
