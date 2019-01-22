using System.Collections.Generic;
using System.Linq;
using ITour.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ITour.Pages.Orders
{
    public class ChoiceCustomerModel : PageModel
    {
        private readonly ITour.Data.ApplicationDbContext _context;

        public ChoiceCustomerModel(ITour.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public JsonResult OnGet(string term)
        {
            List<Customer> customers = _context.Customers.Include(c => c.Person).Include(c => c.CustomerCompany)
                .Where(c => c.Person.Surname.Contains(term) || c.CustomerCompany.Name.Contains(term))
                .AsNoTracking().ToList();

            return customers.Count == 0 
                ? new JsonResult(new { label = "Не найден..." }) 
                : new JsonResult(customers.Select(t => new { value = t.Id, label = t.Name }));
        }
    }
}