using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Services.AccomodationServices.Hotels
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Hotel Hotel { get; set; }

        [BindProperty, Display(Name = "Страна")]
        public Country Country { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Hotel = await _context.Hotels
                .Include(h => h.Resort).ThenInclude(r => r.Country).FirstOrDefaultAsync(m => m.Id == id);

            if (Hotel == null)
            {
                return NotFound();
            }

            Country = Hotel.Resort?.Country;

            IQueryable<Resort> Resorts = _context.Resorts;
            if (Country != null)
                Resorts = Resorts.Where(r => r.CountryId == Country.Id);

            ViewData["CountryId"] = new SelectList(_context.Countries.OrderBy(c => c.Name).AsNoTracking(), "Id", "Name");
            ViewData["ResortId"] = new SelectList(Resorts.OrderBy(r => r.Name).AsNoTracking(), "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Hotel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExists(Hotel.Id))
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

        private bool HotelExists(Guid id)
        {
            return _context.Hotels.Any(e => e.Id == id);
        }

        public JsonResult OnGetCascadingResorts(Guid countryId)
        {
            List<Resort> resortList = new List<Resort>();
            resortList = _context.Resorts.Where(r => r.CountryId == countryId).AsNoTracking().ToList();
            return new JsonResult(new SelectList(resortList, "Id", "Name"));
        }
    }
}
