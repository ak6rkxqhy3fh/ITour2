using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Services.Options;

namespace ITour.Pages.Admin.Options
{
    public class IndexModel : PageModel
    {
        private readonly ITour.Data.ApplicationDbContext _context;

        public IndexModel(ITour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<AppOptions> AppOptions { get;set; }

        public async Task OnGetAsync()
        {
            AppOptions = await _context.AppOptions.ToListAsync();
        }
    }
}
