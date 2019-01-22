using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Services.AccomodationServices.Resorts
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Resort Resort { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Resort = await _context.Resorts.Include(r => r.Country)
                .AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (Resort == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
