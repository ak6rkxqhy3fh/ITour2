using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.AppCompanies.Companies.AgencyCompanies.Licenses
{
    public class EditModel : PageModel
    {
        private readonly ITour.Data.ApplicationDbContext _context;

        public EditModel(ITour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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
           ViewData["AgencyCompanyId"] = new SelectList(_context.AgencyCompanies, "Id", "Name");
           ViewData["AgencyOfficeId"] = new SelectList(_context.AgencyOffices, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(License).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LicenseExists(License.Id))
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

        private bool LicenseExists(Guid id)
        {
            return _context.Licenses.Any(e => e.Id == id);
        }
    }
}
