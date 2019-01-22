using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.AppCompanies.Companies.PartnerCompanies
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ITour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<PartnerCompany> PartnerCompany { get;set; }

        public async Task OnGetAsync()
        {
            PartnerCompany = await _context.PartnerCompanies
                .Include(p => p.Person).ToListAsync();
        }
    }
}
