using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Reports.ComingDeparture
{
    public class EditModel : PageModel
    {
        private readonly ITour.Data.ApplicationDbContext _context;

        public EditModel(ITour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TransportService TransportService { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TransportService = await _context.TransportServices
                .Include(t => t.CurrencyType)
                .Include(t => t.Order)
                .Include(t => t.PacketTourService)
                .Include(t => t.TransportType).FirstOrDefaultAsync(m => m.Id == id);

            if (TransportService == null)
            {
                return NotFound();
            }
           ViewData["CurrencyTypeId"] = new SelectList(_context.CurrencyTypes, "Id", "Name");
           ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Number");
           ViewData["PacketTourServiceId"] = new SelectList(_context.PacketTourServices, "Id", "Id");
           ViewData["TransportTypeId"] = new SelectList(_context.TransportTypes, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(TransportService).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransportServiceExists(TransportService.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TransportServiceExists(Guid id)
        {
            return _context.TransportServices.Any(e => e.Id == id);
        }
    }
}
