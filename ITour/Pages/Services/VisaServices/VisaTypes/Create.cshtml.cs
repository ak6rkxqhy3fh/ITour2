using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ITour.Data;
using ITour.Models;
using ITour.Services.Tenants;

namespace ITour.Pages.Services.VisaServices.VisaTypes
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
        public VisaType VisaType { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            VisaType.TenantId = _tenantProvider.Tenant.Id;
            _context.VisaTypes.Add(VisaType);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}