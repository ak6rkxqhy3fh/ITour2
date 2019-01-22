using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Services.VisaServices.VisaTypes
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<VisaType> VisaType { get;set; }

        public async Task OnGetAsync()
        {
            VisaType = await _context.VisaTypes.ToListAsync();
        }
    }
}
