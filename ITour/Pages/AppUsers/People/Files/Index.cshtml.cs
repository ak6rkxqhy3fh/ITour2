using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.AppUsers.People.Files
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<PersonFile> PersonFile { get;set; }

        public Person Person { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? personId)
        {
            if (personId == null)
                return NotFound();

            Person = await _context.People.FindAsync(personId);

            if (Person == null)
                return NotFound();

            PersonFile = await _context.PersonFile.Where(pf => pf.PersonId == personId)
                .Include(p => p.Person).AsNoTracking().ToListAsync();

            return Page();
        }
    }
}
