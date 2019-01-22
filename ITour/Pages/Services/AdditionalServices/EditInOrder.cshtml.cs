using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Services.AdditionalServices
{
    public class EditInOrderModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditInOrderModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AdditionalService AdditionalService { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
                return NotFound();

            AdditionalService = await _context.AdditionalServices.FirstOrDefaultAsync(m => m.Id == id);

            if (AdditionalService == null)
                return NotFound();

           ViewData["CurrencyTypeId"] = new SelectList(_context.CurrencyTypes.AsNoTracking(), "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
                        
            AdditionalService.Cost = AdditionalService.Cost ?? 0;
            _context.Attach(AdditionalService).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdditionalServiceExists(AdditionalService.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            string returnPage = (string)TempData["ReturnPage"];
            Guid orderId = (Guid)TempData["OrderId"];

            return RedirectToPage(returnPage, "", new { id = orderId }, "Services");
        }

        private bool AdditionalServiceExists(Guid id)
        {
            return _context.AdditionalServices.Any(e => e.Id == id);
        }
    }
}
