﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.AppCompanies.Companies.PartnerCompanies
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public PartnerCompany PartnerCompany { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PartnerCompany = await _context.PartnerCompanies
                .Include(p => p.Person).FirstOrDefaultAsync(m => m.Id == id);

            if (PartnerCompany == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
