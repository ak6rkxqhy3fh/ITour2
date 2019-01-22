using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;
using ITour.Utilities;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITour.Pages.AppCompanies.Companies.CustomerCompanies
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CustomerCompany> CustomerCompany { get;set; }

        [BindProperty(SupportsGet = true)]
        public CustomerCompanyFilter CustomerCompanyFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public CustomerCompanyPaginate CustomerCompanyPaginate { get; set; }

        public async Task OnGetAsync()
        {
            CustomerCompany = await _context.CustomerCompanies
                .Include(c => c.Person).ToListAsync();

            IQueryable<CustomerCompany> customerCompanyIQ = _context.CustomerCompanies;

            customerCompanyIQ = CustomerCompanyFilter.Process(customerCompanyIQ);

            customerCompanyIQ = CustomerCompanyPaginate.Process(customerCompanyIQ);

            CustomerCompany = await customerCompanyIQ.OrderBy(cc => cc.Name)
                .Include(c => c.Person).ToListAsync();

            ViewData["PageSize"] = new SelectList(CustomerCompanyPaginate.PageSizeDictionary, "Key", "Value", CustomerCompanyPaginate.PageSize);
        }
    }

    public class CustomerCompanyFilter
    {
        public string Name { get; set; }

        public IQueryable<CustomerCompany> Process(IQueryable<CustomerCompany> customerCompanyIQ)
        {
            if (!string.IsNullOrEmpty(Name))
                customerCompanyIQ = customerCompanyIQ.Where(cc => cc.Name.Contains(Name));
            return customerCompanyIQ;
        }

        public bool NotAllParamsIsNull => Name != null;
    }

    public class CustomerCompanyPaginate : Paginate<CustomerCompany> { }
}
