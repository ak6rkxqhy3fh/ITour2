using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;
using ITour.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITour.Pages.AppCompanies.TouroperatorCompanies
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<TouroperatorCompany> TouroperatorCompany { get;set; }

        [BindProperty(SupportsGet = true)]
        public TouroperatorCompanyFilter TouroperatorCompanyFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public TouroperatorCompanySort TouroperatorCompanySort { get; set; }

        [BindProperty(SupportsGet = true)]
        public TouroperatorCompanyPaginate TouroperatorCompanyPaginate { get; set; }
        [TempData]
        public int? TouroperatorCompanyPageSize { get; set; }

        public async Task OnGetAsync()
        {
            IQueryable<TouroperatorCompany> touroperatorCompanyIQ = _context.TouroperatorCompanies
                .Include(tc => tc.TouroperatorBrands).ThenInclude(tb => tb.TouroperatorBrand);

            touroperatorCompanyIQ = TouroperatorCompanyFilter.Process(touroperatorCompanyIQ);
            touroperatorCompanyIQ = TouroperatorCompanySort.Process(touroperatorCompanyIQ);
            touroperatorCompanyIQ = TouroperatorCompanyPaginate.Process(touroperatorCompanyIQ, TouroperatorCompanyPageSize);

            TouroperatorCompany = await touroperatorCompanyIQ.AsNoTracking().ToListAsync();

            ViewData["FilterTouroperatorBrandId"] = new SelectList(_context.TouroperatorBrands.OrderBy(tb => tb.Name).AsNoTracking(), "Id", "Name");
            ViewData["PageSize"] = new SelectList(TouroperatorCompanyPaginate.PageSizeDictionary, "Key", "Value", TouroperatorCompanyPaginate.PageSize);
            TouroperatorCompanyPageSize = TouroperatorCompanyPaginate.PageSize;
            TempData.Keep("TouroperatorCompanyPageSize");
        }        
    }

    public class TouroperatorCompanyFilter
    {
        [Display(Name = "Реестровый № или название")]
        public string RagistryNumberName { get; set; }

        [Display(Name = "Туроператор")]
        public Guid? TouroperatorBrandId { get; set; }

        public IQueryable<TouroperatorCompany> Process(IQueryable<TouroperatorCompany> touroperatorCompanyIQ)
        {
            if (!String.IsNullOrEmpty(RagistryNumberName))
                touroperatorCompanyIQ = touroperatorCompanyIQ.
                    Where(tc => tc.RegistryNumber.Contains(RagistryNumberName) || tc.Name.Contains(RagistryNumberName));

            if (TouroperatorBrandId != null)
                touroperatorCompanyIQ = touroperatorCompanyIQ   // TODO TouroperatorCompanies.IndexModel: Проверить запрос!!!
                    .Where(tc => tc.TouroperatorBrands.Where(tb => tb.TouroperatorBrandId == TouroperatorBrandId).Any()); 

            return touroperatorCompanyIQ;
        }

        public bool NotAllParamsIsNull => RagistryNumberName != null || TouroperatorBrandId != null;
    }

    public enum TouroperatorCompanySortState
    {
        RegistryNumberAsc = 1, RegistryNumberDesc = 2,
        NameAsc = 3, NameDesc = 4,
        FinGaranteeExpirationDateAsc = 5, FinGaranteeExpirationDateDesc = 6
    }

    public class TouroperatorCompanySort
    {
        public TouroperatorCompanySortState SortOrder { get; set; }
        public TouroperatorCompanySortState SortState { get; set; }
        public TouroperatorCompanySortState RegistryNumberSort { get; set; }
        public TouroperatorCompanySortState NameSort { get; set; }
        public TouroperatorCompanySortState FinGaranteeExpirationDateSort { get; set; }

        public IQueryable<TouroperatorCompany> Process(IQueryable<TouroperatorCompany> touroperatorCompanyIQ)
        {
            if (SortOrder == 0)
                SortOrder = SortState;
            else
                SortState = SortOrder;

            RegistryNumberSort = SortOrder == TouroperatorCompanySortState.RegistryNumberAsc ? TouroperatorCompanySortState.RegistryNumberDesc : TouroperatorCompanySortState.RegistryNumberAsc;
            NameSort = SortOrder == TouroperatorCompanySortState.NameAsc ? TouroperatorCompanySortState.NameDesc : TouroperatorCompanySortState.NameAsc;
            FinGaranteeExpirationDateSort = SortOrder == TouroperatorCompanySortState.FinGaranteeExpirationDateAsc ? 
                TouroperatorCompanySortState.FinGaranteeExpirationDateDesc : TouroperatorCompanySortState.FinGaranteeExpirationDateAsc;

            switch (SortOrder)
            {
                case TouroperatorCompanySortState.RegistryNumberAsc:
                    touroperatorCompanyIQ = touroperatorCompanyIQ.OrderBy(o => o.RegistryNumber);
                    break;                
                case TouroperatorCompanySortState.RegistryNumberDesc:
                    touroperatorCompanyIQ = touroperatorCompanyIQ.OrderByDescending(o => o.RegistryNumber);
                    break;

                case TouroperatorCompanySortState.NameAsc:
                    touroperatorCompanyIQ = touroperatorCompanyIQ.OrderBy(o => o.Name);
                    break;
                case TouroperatorCompanySortState.NameDesc:
                    touroperatorCompanyIQ = touroperatorCompanyIQ.OrderByDescending(o => o.Name);
                    break;

                case TouroperatorCompanySortState.FinGaranteeExpirationDateAsc:
                    touroperatorCompanyIQ = touroperatorCompanyIQ.OrderBy(o => o.FinGaranteeExpirationDateNewPeriod);
                    break;
                case TouroperatorCompanySortState.FinGaranteeExpirationDateDesc:
                    touroperatorCompanyIQ = touroperatorCompanyIQ.OrderByDescending(o => o.FinGaranteeExpirationDateNewPeriod);
                    break;

                //default:
                //    touroperatorCompanyIQ = touroperatorCompanyIQ.OrderBy(o => o.Name);
                //    break;
            }
            return touroperatorCompanyIQ;
        }
    }

    public class TouroperatorCompanyPaginate : Paginate<TouroperatorCompany> { }
}
