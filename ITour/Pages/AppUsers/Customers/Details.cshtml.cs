using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.AppUsers.Customers
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Customer Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer = await _context.Customers
                .Include(c => c.Person).ThenInclude(p => p.ApplicationUser)
                .Include(c => c.Person).ThenInclude(p => p.IdDocument).ThenInclude(d => d.DocumentType)
                .Include(c => c.CustomerCompany)
                .Include(c => c.Manager).ThenInclude(m => m.Person)
                .AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (Customer == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
