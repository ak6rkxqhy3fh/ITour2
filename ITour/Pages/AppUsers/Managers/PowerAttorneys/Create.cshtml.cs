using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;
using ITour.Services.Tenants;

namespace ITour.Pages.AppUsers.Managers.PowerAttorneys
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
        ViewData["ManagerId"] = new SelectList(_context.Managers.Include(m => m.Person).OrderBy(m => m.Person.Surname).AsNoTracking(), "Id", "Name");
            return Page();
        }

        [BindProperty]
        public PowerAttorney PowerAttorney { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            PowerAttorney.TenantId = _tenantProvider.Tenant.Id;
            _context.PowerAttorneys.Add(PowerAttorney);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}