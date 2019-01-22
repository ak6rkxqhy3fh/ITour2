using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Models;

namespace ITour.Pages.AppCompanies.Companies.AgencyOffices
{
    public class IndexModel : PageModel
    {
        private readonly ITour.Data.ApplicationDbContext _context;

        public IndexModel(ITour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<AgencyOffice> AgencyOffice { get;set; }

        public async Task OnGetAsync()
        {
            AgencyOffice = await _context.AgencyOffices
                .Include(a => a.Person).ToListAsync();
        }
    }
}
