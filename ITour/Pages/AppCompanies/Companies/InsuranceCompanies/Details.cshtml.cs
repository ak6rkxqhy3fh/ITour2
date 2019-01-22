using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.AppCompanies.Companies.InsuranceCompanies
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public InsuranceCompany InsuranceCompany { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InsuranceCompany = await _context.InsuranceCompanies
                .Include(i => i.Person).FirstOrDefaultAsync(m => m.Id == id);

            if (InsuranceCompany == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
