using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Models;

namespace ITour.Pages.AppCompanies.Companies.AgencyOffices
{
    public class EditModel : PageModel
    {
        private readonly ITour.Data.ApplicationDbContext _context;

        public EditModel(ITour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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
           //ViewData["PersonId"] = new SelectList(_context.People, "Id", "SurnameInitials");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(AgencyOffice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgencyOfficeExists(AgencyOffice.Id))
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

        private bool AgencyOfficeExists(Guid id)
        {
            return _context.AgencyOffices.Any(e => e.Id == id);
        }
    }
}
