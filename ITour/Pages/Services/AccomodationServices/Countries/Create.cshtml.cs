using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ITour.Models;
using ITour.Data;
using ITour.Services.Tenants;

namespace ITour.Pages.Services.AccomodationServices.Countries
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
            return Page();
        }

        [BindProperty]
        public Country Country { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            Country.TenantId = _tenantProvider.Tenant.Id;
            _context.Countries.Add(Country);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}