using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.AppCompanies.Companies.AgencyCompanies
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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
           ViewData["PersonId"] = new SelectList(_context.People.Where(p=>p.IsEmployee).AsNoTracking(), "Id", "SurnameInitials");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(AgencyCompany).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgencyCompanyExists(AgencyCompany.Id))
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

        private bool AgencyCompanyExists(Guid id)
        {
            return _context.AgencyCompanies.Any(e => e.Id == id);
        }
    }
}
