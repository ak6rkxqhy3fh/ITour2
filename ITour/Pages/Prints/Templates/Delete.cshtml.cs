using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Prints.Templates
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
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

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PrintTemplate = await _context.PrintTemplates.FindAsync(id);

            if (PrintTemplate != null)
            {
               PrintTemplate.IsDeleted = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
