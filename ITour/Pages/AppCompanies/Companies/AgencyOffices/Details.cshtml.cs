using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Models;

namespace ITour.Pages.AppCompanies.Companies.AgencyOffices
{
    public class DetailsModel : PageModel
    {
        private readonly ITour.Data.ApplicationDbContext _context;

        public DetailsModel(ITour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public AgencyOffice AgencyOffice { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AgencyOffice = await _context.AgencyOffices
                .Include(a => a.Person).FirstOrDefaultAsync(m => m.Id == id);

            if (AgencyOffice == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
