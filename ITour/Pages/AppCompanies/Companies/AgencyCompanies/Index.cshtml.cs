using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.AppCompanies.Companies.AgencyCompanies
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<AgencyCompany> AgencyCompany { get;set; }

        public async Task OnGetAsync()
        {
            AgencyCompany = await _context.AgencyCompanies
                .Include(a => a.Person).ToListAsync();
        }
    }
}
