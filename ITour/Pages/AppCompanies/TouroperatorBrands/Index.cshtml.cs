using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.AppCompanies.TouroperatorBrands
{
    public class IndexModel : PageModel
    {
        private readonly ITour.Data.ApplicationDbContext _context;

        public IndexModel(ITour.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<TouroperatorBrand> TouroperatorBrand { get;set; }

        // TODO TouroperatorBrands/Index: раскрыть туропертора в которого добавили юр. лицо, Позиционировать по центру
        public async Task OnGetAsync(Guid? id)
        {
            TouroperatorBrand = await _context.TouroperatorBrands
                .Include(tb => tb.TouroperatorCompanies).ThenInclude(tc => tc.TouroperatorCompany)
                .OrderBy(tb => tb.Name).AsNoTracking().ToListAsync();
        }

        // TODO TouroperatorBrands/Index: раскрыть туропертора из которого удалили юр. лицо, Позиционировать по центру
        public async Task OnGetDetachAsync(Guid touroperatorCompanyId)  
        {
            TouroperatorBrandCompany touroperatorBrandCompany = await _context.TouroperatorBrandCompanies.FindAsync(touroperatorCompanyId);
            _context.TouroperatorBrandCompanies.Remove(touroperatorBrandCompany);
            _context.SaveChanges();

            TouroperatorBrand = await _context.TouroperatorBrands
                .Include(tb => tb.TouroperatorCompanies).ThenInclude(tc => tc.TouroperatorCompany)
                .OrderBy(tb => tb.Name).AsNoTracking().ToListAsync();
        }
    }
}
