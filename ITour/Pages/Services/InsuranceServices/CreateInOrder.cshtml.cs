using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;
using ITour.Services.Tenants;

namespace ITour.Pages.Services.InsuranceServices
{
    public class CreateInOrderModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ITenantProvider _tenantProvider;

        public CreateInOrderModel(ApplicationDbContext context, ITenantProvider tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }

        public IActionResult OnGet()
        {
            ViewData["CurrencyTypeId"] = new SelectList(_context.CurrencyTypes.AsNoTracking(), "Id", "Name");           
            ViewData["InsuranceCompanyId"] = new SelectList(_context.InsuranceCompanies.AsNoTracking(), "Id", "Name");
            ViewData["InsuranceTypeId"] = new SelectList(_context.InsuranceTypes.AsNoTracking(), "Id", "Name");
            return Page();
        }

        [BindProperty]
        public InsuranceService InsuranceService { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            string returnPage = (string)TempData["ReturnPage"];
            Guid orderId = (Guid)TempData["OrderId"];

            InsuranceService.TenantId = _tenantProvider.Tenant.Id;
            InsuranceService.OrderId = orderId;
            InsuranceService.Cost = InsuranceService.Cost ?? 0;
            _context.InsuranceServices.Add(InsuranceService);
            await _context.SaveChangesAsync();

            return RedirectToPage(returnPage, "", new { id = orderId }, "Services");
        }
    }
}