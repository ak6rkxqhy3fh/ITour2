using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Models;

namespace ITour.Pages.Admin.Orders.OrderStatuses
{
    public class DetailsModel : PageModel
    {
        private readonly ITour.Data.ApplicationDbContext _context;

        public DetailsModel(ITour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public OrderStatus OrderStatus { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            OrderStatus = await _context.OrderStatuses.IgnoreQueryFilters().AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (OrderStatus == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
