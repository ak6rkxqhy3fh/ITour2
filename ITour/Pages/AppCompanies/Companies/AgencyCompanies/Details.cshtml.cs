using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.AppCompanies.Companies.AgencyCompanies
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public AgencyCompany AgencyCompany { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AgencyCompany = await _context.AgencyCompanies
                .Include(a => a.Person).FirstOrDefaultAsync(m => m.Id == id);

            if (AgencyCompany == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
