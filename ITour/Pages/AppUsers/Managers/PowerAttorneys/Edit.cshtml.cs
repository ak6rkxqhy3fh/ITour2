using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITour.Models;

namespace ITour.Pages.AppUsers.Managers.PowerAttorneys
{
    public class EditModel : PageModel
    {
        private readonly ITour.Data.ApplicationDbContext _context;

        public EditModel(ITour.Data.ApplicationDbContext context)
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
                .Include(p => p.Manager).FirstOrDefaultAsync(m => m.Id == id);

            if (PowerAttorney == null)
            {
                return NotFound();
            }
           ViewData["ManagerId"] = new SelectList(_context.Managers.Include(m => m.Person).OrderBy(m => m.Person.Surname).AsNoTracking(), "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(PowerAttorney).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PowerAttorneyExists(PowerAttorney.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PowerAttorneyExists(Guid id)
        {
            return _context.PowerAttorneys.Any(e => e.Id == id);
        }
    }
}
