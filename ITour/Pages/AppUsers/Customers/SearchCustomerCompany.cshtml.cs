using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.AppUsers.Customers
{
    public class SearchCustomerCompanyModel : PageModel
    {
        ApplicationDbContext _context;

        public SearchCustomerCompanyModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public JsonResult OnGet(string term)
        {
            List<CustomerCompany> customerCompanies = _context.CustomerCompanies
                .Where(c => c.Name.Contains(term))
                .AsNoTracking().ToList();

            return customerCompanies.Count == 0
                ? new JsonResult(new { label = "Не найден..." })
                : new JsonResult(customerCompanies.Select(t => new { value = t.Id, label = t.Name }));
        }
    }
}