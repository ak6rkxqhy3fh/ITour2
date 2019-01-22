using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Payments.PaymentForms
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<PaymentForm> PaymentForm { get;set; }

        public async Task OnGetAsync()
        {
            PaymentForm = await _context.PaymentForms.AsNoTracking().ToListAsync();
        }
    }
}
