using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.AppUsers.Managers.PowerAttorneys
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PowerAttorney PowerAttorney { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PowerAttorney = await _context.PowerAttorneys
                .Include(p => p.Manager).ThenInclude(m => m.Person)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (PowerAttorney == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PowerAttorney = await _context.PowerAttorneys.FindAsync(id);

            if (PowerAttorney != null)
            {
                PowerAttorney.IsDeleted = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
