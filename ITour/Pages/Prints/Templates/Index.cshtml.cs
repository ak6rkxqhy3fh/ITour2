using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Prints.Templates
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public IList<PrintTemplate> PrintTemplate { get; set; }

        public async Task OnGetAsync()
        {
            PrintTemplate = await _context.PrintTemplates.OrderBy(p => p.Sequence).AsNoTracking().ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(string sortableResult)
        {
            var printTemplates = await _context.PrintTemplates.OrderBy(p => p.Sequence).ToListAsync();            

            foreach (var printTemplate in printTemplates)
            {
                printTemplate.IsActive = PrintTemplate.First(p => p.Id == printTemplate.Id).IsActive;
            }

            if (!string.IsNullOrEmpty(sortableResult))
            {
                int[] sequence = Array.ConvertAll(sortableResult.Split(","), int.Parse);

                for (int i = 0; i < printTemplates.Count; i++)
                {
                    printTemplates[sequence[i]].Sequence = i + 1;
                }
            }

                _context.PrintTemplates.UpdateRange(printTemplates);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }

        // TODO LINQ query WHERE ... IN ...
        //Guid[]  isActive ...
        //List<Print> prints = _context.Prints.Where(p => isActive.Contains(p.Id)).ToList();

        // TODO String to Number Array
        //int[] sequence = sequenceString.Split(",").Select(s => int.Parse(s)).ToArray();
        //int[] sequence = sequenceString.Split(",").Select(int.Parse).ToArray();

    }
}
