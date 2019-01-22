using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Models;

namespace ITour.Pages.AppCompanies.Companies.InsuranceCompanies
{
    public class EditModel : PageModel
    {
        private readonly ITour.Data.ApplicationDbContext _context;

        public EditModel(ITour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(InsuranceCompany).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InsuranceCompanyExists(InsuranceCompany.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool InsuranceCompanyExists(Guid id)
        {
            return _context.InsuranceCompanies.Any(e => e.Id == id);
        }
    }
}
