using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Models;

namespace ITour.Pages.AppUsers.Managers.PowerAttorneys
{
    public class IndexModel : PageModel
    {
        private readonly ITour.Data.ApplicationDbContext _context;

        public IndexModel(ITour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<PowerAttorney> PowerAttorney { get;set; }

        public async Task OnGetAsync()
        {
            PowerAttorney = await _context.PowerAttorneys
                .Include(pa => pa.Manager).ThenInclude(m => m.Person)
                .AsNoTracking().ToListAsync();
        }
    }
}
