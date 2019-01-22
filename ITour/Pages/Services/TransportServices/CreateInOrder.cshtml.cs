using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;
using ITour.Services.Tenants;

namespace ITour.Pages.Services.TransportServices
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
            ViewData["TransportTypeId"] = new SelectList(_context.TransportTypes.AsNoTracking(), "Id", "Name");
            return Page();
        }

        [BindProperty]
        public TransportService TransportService { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            string returnPage = (string)TempData["ReturnPage"];
            Guid orderId = (Guid)TempData["OrderId"];

            TransportService.TenantId = _tenantProvider.Tenant.Id;
            TransportService.OrderId = orderId;
            TransportService.Cost = TransportService.Cost ?? 0;
            _context.TransportServices.Add(TransportService);
            await _context.SaveChangesAsync();

            return RedirectToPage(returnPage, "", new { id = orderId }, "Services");
        }
    }
}