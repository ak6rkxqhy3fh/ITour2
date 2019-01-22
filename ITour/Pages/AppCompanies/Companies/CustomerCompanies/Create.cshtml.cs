using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ITour.Data;
using ITour.Models;
using ITour.Services.Tenants;

namespace ITour.Pages.AppCompanies.Companies.CustomerCompanies
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
        //ViewData["PersonId"] = new SelectList(_context.People, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public CustomerCompany CustomerCompany { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            CustomerCompany.TenantId = _tenantProvider.Tenant.Id;
            _context.CustomerCompanies.Add(CustomerCompany);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}