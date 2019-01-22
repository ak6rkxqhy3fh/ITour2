using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ITour.Models;

namespace ITour.Pages.Admin.Orders.OrderStatuses
{
    public class CreateModel : PageModel
    {
        private readonly ITour.Data.ApplicationDbContext _context;

        public CreateModel(ITour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public OrderStatus OrderStatus { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.OrderStatuses.Add(OrderStatus);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}