using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Payments.OutgoingPayments
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
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
                .Include(o => o.PaymentForm)
                .Include(o => o.PaymentType)
                .Include(o => o.Order)
                .Include(o => o.PartnerCompany).FirstOrDefaultAsync(m => m.Id == outgoingPaymentId);

            if (OutgoingPayment == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? outgoingPaymentId)
        {
            if (outgoingPaymentId == null)
            {
                return NotFound();
            }

            OutgoingPayment = await _context.OutgoingPayments.FindAsync(outgoingPaymentId);

            if (OutgoingPayment != null)
            {
                _context.OutgoingPayments.Remove(OutgoingPayment);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("../Index");
        }
    }
}
