using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Services.InsuranceServices.InsuranceTypes
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InsuranceType InsuranceType { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InsuranceType = await _context.InsuranceTypes.FirstOrDefaultAsync(m => m.Id == id);

            if (InsuranceType == null)
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

            InsuranceType = await _context.InsuranceTypes.FindAsync(id);

            if (InsuranceType != null)
            {
                if (!InsuranceType.IsSystem)
                {
                    InsuranceType.IsDeleted = true;
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
