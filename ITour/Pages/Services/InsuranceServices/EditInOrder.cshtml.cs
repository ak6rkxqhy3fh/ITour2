using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Services.InsuranceServices
{
    public class EditInOrderModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditInOrderModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InsuranceService InsuranceService { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
                return NotFound();

            InsuranceService = await _context.InsuranceServices.FirstOrDefaultAsync(m => m.Id == id);

            if (InsuranceService == null)
                return NotFound();

           ViewData["CurrencyTypeId"] = new SelectList(_context.CurrencyTypes.AsNoTracking(), "Id", "Name");
           ViewData["InsuranceCompanyId"] = new SelectList(_context.InsuranceCompanies.AsNoTracking(), "Id", "Name");
           ViewData["InsuranceTypeId"] = new SelectList(_context.InsuranceTypes.AsNoTracking(), "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            InsuranceService.Cost = InsuranceService.Cost ?? 0;
            _context.Attach(InsuranceService).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InsuranceServiceExists(InsuranceService.Id))
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

        private bool InsuranceServiceExists(Guid id)
        {
            return _context.InsuranceServices.Any(e => e.Id == id);
        }
    }
}
