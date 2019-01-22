using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;
using ITour.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITour.Pages.AppUsers.Customers
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Customer> Customer { get;set; }

        [BindProperty(SupportsGet = true)]
        public CustomerFilter CustomerFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public CustomerPaginate CustomerPaginate { get; set; }

        public async Task OnGetAsync()
        {
            IQueryable<Customer> customerIQ = _context.Customers
                .Include(c => c.Person).ThenInclude(p => p.ApplicationUser)
                .Include(c => c.CustomerCompany)
                .Include(c => c.Manager).ThenInclude(m => m.Person);

            customerIQ = CustomerFilter.Process(customerIQ);

            customerIQ = CustomerPaginate.Process(customerIQ);

            Customer = await customerIQ.OrderBy(c => c.Person.Surname).AsNoTracking().ToListAsync();

            ViewData["FilterManagerId"] = new SelectList(_context.Managers.Include(m => m.Person).OrderBy(m => m.Person.Surname).AsNoTracking(), "Id", "Name");
            ViewData["PageSize"] = new SelectList(CustomerPaginate.PageSizeDictionary, "Key", "Value", CustomerPaginate.PageSize);
        }
    }

    public class CustomerFilter
    {

        [Display(Name = "Заказчик")]
        public string CustomerName { get; set; }
        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Менеджер")]
        public Guid? ManagerId { get; set; }


        public IQueryable<Customer> Process(IQueryable<Customer> customerIQ)
        {
            if (CustomerName != null)
                customerIQ = customerIQ.Where(c =>
                c.CustomerCompany.Name.Contains(CustomerName)
                || c.Person.Surname.Contains(CustomerName) 
                || c.Person.Firstname.Contains(CustomerName)
                || c.Person.Middlename.Contains(CustomerName)
                );

            if (!string.IsNullOrEmpty(PhoneNumber))
                customerIQ = customerIQ.Where(p => p.Person.ApplicationUser.PhoneNumber.Contains(PhoneNumber));

            if (!string.IsNullOrEmpty(Email))
                customerIQ = customerIQ.Where(p => p.Person.ApplicationUser.Email.Contains(Email));

            if (ManagerId != null)
                customerIQ = customerIQ.Where(p => p.ManagerId == ManagerId);

            return customerIQ;
        }
        public bool NotAllParamsIsNull => CustomerName != null || PhoneNumber != null || Email != null || ManagerId != null;
    }

    public class CustomerPaginate : Paginate<Customer> { }
}
