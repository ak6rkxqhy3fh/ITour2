using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.AppCompanies.TouroperatorCompanies
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TouroperatorCompany TouroperatorCompany { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
                return NotFound();

            TouroperatorCompany = await _context.TouroperatorCompanies
                .Include(tc => tc.TouroperatorBrands)
                    .ThenInclude(tb => tb.TouroperatorBrand)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (TouroperatorCompany == null)
                return NotFound();

            //В результате global query filter по TenantId должнен остаться только один TouroperatorBrandCompany
            if (TouroperatorCompany.TouroperatorBrands.Count == 1)
            {
                ViewData["TouroperatorBrandCompanyId"] = TouroperatorCompany.TouroperatorBrands[0].Id;
                ViewData["TouroperatorBrandCompanyName"] = TouroperatorCompany.TouroperatorBrands[0].TouroperatorBrand.Name;
            }
            else if (TouroperatorCompany.TouroperatorBrands.Count > 1)
                throw new Exception("TouroperatorCompany.TouroperatorBrands.Count > 1");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id, Guid? touroperatorBrandCompanyId)
        {
            if (id == null)
                return NotFound();

            TouroperatorCompany = await _context.TouroperatorCompanies.FindAsync(id);

            if (TouroperatorCompany != null && !TouroperatorCompany.IsOpenData)
            {
                TouroperatorCompany.IsDeleted = true;

                if (touroperatorBrandCompanyId != null)
                {
                    TouroperatorBrandCompany touroperatorBrandCompany = await _context.TouroperatorBrandCompanies.FindAsync(touroperatorBrandCompanyId);
                    _context.TouroperatorBrandCompanies.Remove(touroperatorBrandCompany);
                }

                    await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
