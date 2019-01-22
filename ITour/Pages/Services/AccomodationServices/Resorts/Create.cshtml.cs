using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ITour.Models;
using ITour.Data;
using ITour.Services.Tenants;

namespace ITour.Pages.Services.AccomodationServices.Resorts
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
        ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Resort Resort { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Resort.TenantId = _tenantProvider.Tenant.Id;
            _context.Resorts.Add(Resort);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}