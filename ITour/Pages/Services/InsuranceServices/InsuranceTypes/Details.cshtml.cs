using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Services.InsuranceServices.InsuranceTypes
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
