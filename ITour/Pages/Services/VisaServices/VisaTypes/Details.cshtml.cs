using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Services.VisaServices.VisaTypes
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public VisaType VisaType { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            VisaType = await _context.VisaTypes.FirstOrDefaultAsync(m => m.Id == id);

            if (VisaType == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
