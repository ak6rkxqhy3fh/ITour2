using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Models;

namespace ITour.Pages.Admin.Orders.OrderStatuses
{
    public class DeleteModel : PageModel
    {
        private readonly ITour.Data.ApplicationDbContext _context;

        public DeleteModel(ITour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public OrderStatus OrderStatus { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            OrderStatus = await _context.OrderStatuses.IgnoreQueryFilters().FirstOrDefaultAsync(m => m.Id == id);

            if (OrderStatus == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            OrderStatus = await _context.OrderStatuses.FindAsync(id);

            if (OrderStatus != null)
            {
                _context.OrderStatuses.Remove(OrderStatus);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
