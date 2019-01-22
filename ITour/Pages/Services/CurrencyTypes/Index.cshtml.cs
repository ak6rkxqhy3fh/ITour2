using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Services.CurrencyTypes
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CurrencyType> CurrencyType { get;set; }

        public async Task OnGetAsync()
        {
            CurrencyType = await _context.CurrencyTypes.ToListAsync();
        }
    }
}
