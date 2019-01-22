using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ITour.Data;
using ITour.Models;
using ITour.Services.Tenants;

namespace ITour.Pages.Profits.OutgoingPayments
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
        public OutgoingPayment OutgoingPayment { get; set; }

        [BindProperty]
        public Guid OrderId { get; set; }

        public IActionResult OnGet(Guid orderId)
        {
            ViewData["PaymentFormId"] = new SelectList(_context.PaymentForms, "Id", "Name");
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentTypes, "Id", "Name");
            ViewData["PartnerCompanyId"] = new SelectList(_context.PartnerCompanies, "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            OutgoingPayment.TenantId = _tenantProvider.Tenant.Id;
            OutgoingPayment.OrderId = OrderId;
            _context.OutgoingPayments.Add(OutgoingPayment);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Index");
        }
    }
}