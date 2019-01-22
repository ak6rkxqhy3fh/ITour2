using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ITour.Data;
using ITour.Models;
using ITour.Services.Tenants;

namespace ITour.Pages.Prints.Templates
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
        public PrintTemplate PrintTemplate { get; set; }

        public async Task<IActionResult> OnPostAsync(bool? isClose)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            PrintTemplate.TenantId = _tenantProvider.Tenant.Id;
            PrintTemplate.IsActive = true;
            _context.PrintTemplates.Add(PrintTemplate);
            await _context.SaveChangesAsync();

            if (isClose ?? false )
                return RedirectToPage("./Index");
            else
                return RedirectToPage("./Edit", new { PrintTemplate.Id });
        }
    }
}