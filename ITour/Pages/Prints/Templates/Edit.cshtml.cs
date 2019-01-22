using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Prints.Templates
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PrintTemplate PrintTemplate { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PrintTemplate = await _context.PrintTemplates.FirstOrDefaultAsync(m => m.Id == id);

            if (PrintTemplate == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool? isClose)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(PrintTemplate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrintExists(PrintTemplate.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            if (isClose ?? false)
                return RedirectToPage("./Index");
            else
                return Page();
        }

        private bool PrintExists(Guid id)
        {
            return _context.PrintTemplates.Any(e => e.Id == id);
        }
    }
}
