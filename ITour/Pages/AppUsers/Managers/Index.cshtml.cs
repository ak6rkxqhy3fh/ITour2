using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Models;
using System.Linq;

namespace ITour.Pages.AppUsers.Managers
{
    public class IndexModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;

        public IndexModel(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Manager> Manager { get;set; }

        public async Task OnGetAsync()
        {
            Manager = await _context.Managers                
                .Include(m => m.Person).ThenInclude(p => p.ApplicationUser)
                .Include(m => m.AgencyOffice)
                .OrderBy(p => p.Person.Surname)
                .AsNoTracking().ToListAsync();
        }
    }
}
