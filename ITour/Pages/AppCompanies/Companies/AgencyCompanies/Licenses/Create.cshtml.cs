using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ITour.Data;
using ITour.Models;
using ITour.Services.Tenants;

namespace ITour.Pages.AppCompanies.Companies.AgencyCompanies.Licenses
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
        ViewData["AgencyCompanyId"] = new SelectList(_context.AgencyCompanies, "Id", "Name");
        ViewData["AgencyOfficeId"] = new SelectList(_context.AgencyOffices, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public License License { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            License.TenantId = _tenantProvider.Tenant.Id;
            _context.Licenses.Add(License);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}