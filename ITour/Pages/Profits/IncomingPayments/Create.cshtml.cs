using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ITour.Data;
using ITour.Models;
using ITour.Services.Tenants;

namespace ITour.Pages.Profits.IncomingPayments
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

        [BindProperty]
        public IncomingPayment IncomingPayment { get; set; }

        [BindProperty]
        public Guid OrderId { get; set; }

        public IActionResult OnGet(Guid orderId)
        {
            ViewData["PaymentFormId"] = new SelectList(_context.PaymentForms, "Id", "Name");
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentTypes, "Id", "Name");
            OrderId = orderId;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            IncomingPayment.TenantId = _tenantProvider.Tenant.Id;
            IncomingPayment.OrderId = OrderId;
            _context.IncomingPayments.Add(IncomingPayment);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Index");
        }
    }
}