using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Payments.OutgoingPayments
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public OutgoingPayment OutgoingPayment { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? outgoingPaymentId)
        {
            if (outgoingPaymentId == null)
            {
                return NotFound();
            }

            OutgoingPayment = await _context.OutgoingPayments
                .Include(p => p.PaymentForm)
                .Include(p => p.PaymentType)
                .Include(p => p.PartnerCompany)
                .FirstOrDefaultAsync(m => m.Id == outgoingPaymentId);

            if (OutgoingPayment == null)
            {
                return NotFound();
            }

            ViewData["OrderId"] = TempData.Peek("OrderId");
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

            OutgoingPayment.PaymentAmount = OutgoingPayment.PaymentAmount ?? 0;
            _context.Attach(OutgoingPayment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(OutgoingPayment.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../Index");
        }

        private bool PaymentExists(Guid id)
        {
            return _context.OutgoingPayments.Any(e => e.Id == id);
        }
    }
}