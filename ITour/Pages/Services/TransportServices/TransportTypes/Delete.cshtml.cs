using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Services.TransportServices.TransportTypes
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TransportType TransportType { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TransportType = await _context.TransportTypes.FirstOrDefaultAsync(m => m.Id == id);

            if (TransportType == null)
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

            TransportType = await _context.TransportTypes.FindAsync(id);

            if (TransportType != null)
            {
                if (!TransportType.IsSystem)
                {
                    TransportType.IsDeleted = true;
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
