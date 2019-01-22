using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using ITour.Models;
using ITour.Data;
using ITour.Services.Tenants;

namespace ITour.Pages.Services.AccomodationServices.Hotels
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ITenantProvider _tenantProvider;

        public CreateModel(ApplicationDbContext context, ITenantProvider tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }

        public IActionResult OnGet()
        {
            ViewData["CountryId"] = new SelectList(_context.Countries.OrderBy(c => c.Name ).AsNoTracking(), "Id", "Name");
            ViewData["ResortId"] = new SelectList(_context.Resorts.OrderBy(r => r.Name).AsNoTracking(), "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Hotel Hotel { get; set; }

        [BindProperty, Display(Name ="Страна")]
        public Country Country { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Hotel.TenantId = _tenantProvider.Tenant.Id;
            _context.Hotels.Add(Hotel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public JsonResult OnGetCascadingResorts(Guid countryId)
        {
            List<Resort> resortList = new List<Resort>();
            resortList = _context.Resorts.Where(r => r.CountryId == countryId).AsNoTracking().ToList();
            return new JsonResult(new SelectList(resortList, "Id", "Name"));
        }
    }
}