using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Services.VisaServices
{
    public class EditInOrderModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditInOrderModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public VisaService VisaService { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
                return NotFound();

            VisaService = await _context.VisaServices.FirstOrDefaultAsync(m => m.Id == id);

            if (VisaService == null)
                return NotFound();

            ViewData["CurrencyTypeId"] = new SelectList(_context.CurrencyTypes.AsNoTracking(), "Id", "Name");
            ViewData["VisaTypeId"] = new SelectList(_context.VisaTypes.AsNoTracking(), "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            VisaService.Cost = VisaService.Cost ?? 0;
            _context.Attach(VisaService).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VisaServiceExists(VisaService.Id))
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

        private bool VisaServiceExists(Guid id)
        {
            return _context.VisaServices.Any(e => e.Id == id);
        }
    }
}
