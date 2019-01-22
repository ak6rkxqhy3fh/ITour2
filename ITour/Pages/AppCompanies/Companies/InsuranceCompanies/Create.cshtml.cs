using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ITour.Data;
using ITour.Models;
using ITour.Services.Tenants;

namespace ITour.Pages.AppCompanies.Companies.InsuranceCompanies
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ITenantProvider _tenantProvider;

        public CreateModel(ITour.Data.ApplicationDbContext context, ITenantProvider tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public InsuranceCompany InsuranceCompany { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            InsuranceCompany.TenantId = _tenantProvider.Tenant.Id;
            _context.InsuranceCompanies.Add(InsuranceCompany);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}