using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.AppUsers.Managers
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Manager Manager { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Manager = await _context.Managers                
                .Include(m => m.Person).ThenInclude(p => p.ApplicationUser)
                .Include(m => m.AgencyOffice)
                .AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (Manager == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
