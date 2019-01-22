using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Models;
using ITour.Services.Tenants;

namespace ITour.Pages.Admin.Orders.OrderStatuses
{
    public class IndexModel : PageModel
    {
        private readonly ITour.Data.ApplicationDbContext _context;
        public List<Tenant> Tenants { get; set; }

        public IndexModel(ITour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<OrderStatus> OrderStatus { get;set; }

        public async Task OnGetAsync()
        {
            OrderStatus = await _context.OrderStatuses.OrderBy(os => os.TenantId).ThenBy(os => os.Sequence)
                .AsNoTracking().IgnoreQueryFilters().ToListAsync();
        }
    }
}
