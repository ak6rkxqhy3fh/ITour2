using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Models;

namespace ITour.Pages.AppUsers.Managers.PowerAttorneys
{
    public class DetailsModel : PageModel
    {
        private readonly ITour.Data.ApplicationDbContext _context;

        public DetailsModel(ITour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public PowerAttorney PowerAttorney { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PowerAttorney = await _context.PowerAttorneys
                .Include(p => p.Manager).ThenInclude(m => m.Person)
                .AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (PowerAttorney == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
