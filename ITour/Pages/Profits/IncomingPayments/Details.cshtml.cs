using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Profits.IncomingPayments
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IncomingPayment IncomingPayment { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? incomingPaymentId)
        {
            if (incomingPaymentId == null)
            {
                return NotFound();
            }

            IncomingPayment = await _context.IncomingPayments
                .Include(i => i.PaymentForm)
                .Include(i => i.PaymentType)
                .Include(i => i.Order).FirstOrDefaultAsync(m => m.Id == incomingPaymentId);

            if (IncomingPayment == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
