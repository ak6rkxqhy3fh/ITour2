using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ITour.Data;
using ITour.Models;
using ITour.Services.Tenants;

namespace ITour.Pages.Payments.OutgoingPayments
{
    public class CreateInOrderModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly ITenantProvider _tenantProvider;

        public CreateInOrderModel(Data.ApplicationDbContext context, ITenantProvider tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }

        public IActionResult OnGet()
        {
            OutgoingPayment = new OutgoingPayment();

            ViewData["PaymentFormId"] = new SelectList(_context.PaymentForms, "Id", "Name");
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentTypes, "Id", "Name");
            ViewData["PartnerCompanyId"] = new SelectList(_context.PartnerCompanies, "Id", "Name");

            return Page();
        }

        [BindProperty]
        public OutgoingPayment OutgoingPayment { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string returnPage = (string)TempData["ReturnPage"];
            Guid orderId = (Guid)TempData["OrderId"];

            OutgoingPayment.PaymentAmount = OutgoingPayment.PaymentAmount ?? 0;
            OutgoingPayment.TenantId = _tenantProvider.Tenant.Id;
            OutgoingPayment.OrderId = orderId;
            _context.OutgoingPayments.Add(OutgoingPayment);
            await _context.SaveChangesAsync();

            return RedirectToPage(returnPage, "", new { id = orderId }, "Payments");
        }
    }
}