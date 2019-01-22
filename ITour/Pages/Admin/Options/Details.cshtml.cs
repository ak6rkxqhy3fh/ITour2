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
    public class DetailsModel : PageModel
    {
        private readonly ITour.Data.ApplicationDbContext _context;

        public DetailsModel(ITour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public AppOptions AppOptions { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppOptions = await _context.AppOptions.FirstOrDefaultAsync(m => m.Id == id);

            if (AppOptions == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
