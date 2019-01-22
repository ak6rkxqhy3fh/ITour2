using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;
using System.Linq;

namespace ITour.Pages.Orders.OrderStatuses
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<OrderStatus> OrderStatus { get;set; }

        public async Task OnGetAsync()
        {
            OrderStatus = await _context.OrderStatuses.OrderBy(os => os.Sequence).ThenBy(os => os.Name)
                .AsNoTracking().ToListAsync();
        }
    }
}
