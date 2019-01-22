using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Services.InsuranceServices.InsuranceTypes
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InsuranceType> InsuranceType { get;set; }

        public async Task OnGetAsync()
        {
            InsuranceType = await _context.InsuranceTypes.ToListAsync();
        }
    }
}
