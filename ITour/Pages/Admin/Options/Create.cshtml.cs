using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ITour.Data;
using ITour.Services.Options;

namespace ITour.Pages.Admin.Options
{
    public class CreateModel : PageModel
    {
        private readonly ITour.Data.ApplicationDbContext _context;

        public CreateModel(ITour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public AppOptions AppOptions { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.AppOptions.Add(AppOptions);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}