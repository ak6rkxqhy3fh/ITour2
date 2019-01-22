using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Payments.OutgoingPayments
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
