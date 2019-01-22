using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ITour.Data;
using ITour.Models;
using Microsoft.EntityFrameworkCore;
using ITour.Services.Tenants;

namespace ITour.Pages.AppCompanies.Companies.AgencyCompanies
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ITenantProvider _tenantProvider;

        public CreateModel(ApplicationDbContext context, ITenantProvider tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }

        public IActionResult OnGet()
        {
        ViewData["PersonId"] = new SelectList(_context.People.Where(p => p.IsEmployee).AsNoTracking(), "Id", "SurnameInitials");
            return Page();
        }

        [BindProperty]
        public AgencyCompany AgencyCompany { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            AgencyCompany.TenantId = _tenantProvider.Tenant.Id;
            _context.AgencyCompanies.Add(AgencyCompany);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}