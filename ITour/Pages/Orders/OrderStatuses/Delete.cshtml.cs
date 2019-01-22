using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Orders.OrderStatuses
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
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

            OrderStatus = await _context.OrderStatuses.FirstOrDefaultAsync(m => m.Id == id);

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
                if (!OrderStatus.IsSystem)
                {
                    OrderStatus.IsDeleted = true;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ModelState.AddModelError("SystemType", "Системный тип нельзя удалить");
                    return Page();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
