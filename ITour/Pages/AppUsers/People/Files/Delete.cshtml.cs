using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.AppUsers.People.Files
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PersonFile PersonFile { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id, Guid personId)
        {
            if (id == null)
            {
                return NotFound();
            }

            PersonFile = await _context.PersonFile
                .Include(p => p.Person).FirstOrDefaultAsync(m => m.Id == id);

            if (PersonFile == null)
            {
                return NotFound();
            }

            ViewData["PersonId"] = personId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id, Guid personId)
        {
            if (id == null)
            {
                return NotFound();
            }

            PersonFile = await _context.PersonFile.FindAsync(id);

            if (PersonFile != null)
            {
                PersonFile.IsDeleted = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new { personId });
        }
    }
}
