using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ITour.Data;
using ITour.Models;
using ITour.Services.Tenants;

namespace ITour.Pages.AppCompanies.Companies.PartnerCompanies
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
        ViewData["PersonId"] = new SelectList(_context.People, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public PartnerCompany PartnerCompany { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            PartnerCompany.TenantId = _tenantProvider.Tenant.Id;
            _context.PartnerCompanies.Add(PartnerCompany);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}