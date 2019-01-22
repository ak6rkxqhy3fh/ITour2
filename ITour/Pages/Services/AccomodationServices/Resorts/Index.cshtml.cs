using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ITour.Models;
using ITour.Data;

namespace ITour.Pages.Services.AccomodationServices.Resorts
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Resort> Resort { get; set; }

        [BindProperty(SupportsGet = true)]
        public ResortFilter ResortFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public ResortPaginate ResortPaginate { get; set; }



        public async Task OnGetAsync()
        {
            //Resort = await _context.Resorts.Include(r => r.Country)
            //    .OrderBy(r => r.Country.Name).ThenBy(r => r.Name)
            //    .AsNoTracking().ToListAsync();

            IQueryable<Resort> resortIQ = _context.Resorts.Include(r => r.Country)
                .OrderBy(r => r.Country.Name).ThenBy(r => r.Name);

            resortIQ = ResortFilter.Process(resortIQ);

            resortIQ = ResortPaginate.Process(resortIQ);

            Resort = await resortIQ.AsNoTracking().ToListAsync();

                ViewData["FilterCountryId"] = new SelectList(_context.Countries.OrderBy(c => c.Name).AsNoTracking(), "Id", "Name");
                ViewData["PageSize"] = new SelectList(ResortPaginate.PageSizeDictionary, "Key", "Value", ResortPaginate.PageSize);
        }
    }

    public class ResortFilter
    {
        [Display(Name = "Страна")]
        public Guid? CountryId { get; set; }

        public bool NotAllParamsIsNull => CountryId != null;

        public IQueryable<Resort> Process(IQueryable<Resort> resortIQ)
        {
            if (CountryId != null)
                resortIQ = resortIQ.Where(r => r.CountryId == CountryId);

            return resortIQ;
        }
    }

    public class ResortPaginate
    {
        public int Count { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public IQueryable<Resort> Process(IQueryable<Resort> resortIQ)
        {
            PageSize = PageSize == 0 ? 10 : PageSize;
            PageNumber = PageNumber == 0 ? 1 : PageNumber;

            Count = resortIQ.Count();
            TotalPages = (int)Math.Ceiling(Count / (double)PageSize);

            resortIQ = resortIQ.Skip((PageNumber - 1) * PageSize).Take(PageSize);
            return resortIQ;
        }

        public IEnumerable PageSizeDictionary => new Dictionary<int, string>() { { 5, "5" }, { 10, "10" }, { 20, "20" }, { 50, "50" } };
    }
}
