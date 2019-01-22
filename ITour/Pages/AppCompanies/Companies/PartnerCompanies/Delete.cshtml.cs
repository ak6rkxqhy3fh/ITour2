using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.AppCompanies.Companies.PartnerCompanies
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PartnerCompany PartnerCompany { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PartnerCompany = await _context.PartnerCompanies
                .Include(p => p.Person).FirstOrDefaultAsync(m => m.Id == id);

            if (PartnerCompany == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PartnerCompany = await _context.PartnerCompanies.FindAsync(id);

            if (PartnerCompany != null)
            {
                PartnerCompany.IsDeleted = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
