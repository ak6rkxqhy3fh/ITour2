using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Profits.IncomingPayments
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public IncomingPayment IncomingPayment { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? incomingPaymentId)
        {
            if (incomingPaymentId == null)
            {
                return NotFound();
            }

            IncomingPayment = await _context.IncomingPayments
                .Include(p => p.PaymentForm)
                .Include(p => p.PaymentType)
                .FirstOrDefaultAsync(m => m.Id == incomingPaymentId);

            if (IncomingPayment == null)
            {
                return NotFound();
            }

            ViewData["OrderId"] = TempData.Peek("OrderId");
            ViewData["PaymentFormId"] = new SelectList(_context.PaymentForms, "Id", "Name");
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentTypes, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(IncomingPayment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(IncomingPayment.Id))
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
            return _context.IncomingPayments.Any(e => e.Id == id);
        }
    }
}