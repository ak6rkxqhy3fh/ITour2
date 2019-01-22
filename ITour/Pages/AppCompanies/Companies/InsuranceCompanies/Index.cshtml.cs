using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.AppCompanies.Companies.InsuranceCompanies
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InsuranceCompany> InsuranceCompany { get;set; }

        public async Task OnGetAsync()
        {
            InsuranceCompany = await _context.InsuranceCompanies
                .Include(i => i.Person).ToListAsync();
        }
    }
}
