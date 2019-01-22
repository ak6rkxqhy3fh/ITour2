using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;
using ITour.Services.Tenants;

namespace ITour.Pages.AppUsers.Customers
{
    public class CreateAsCompanyModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ITenantProvider _tenantProvider;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateAsCompanyModel(ApplicationDbContext context, ITenantProvider tenantProvider, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _tenantProvider = tenantProvider;
            _userManager = userManager;
        }

        [BindProperty]
        public Person Person { get; set; }

        [BindProperty]
        public CustomerCompany CustomerCompany { get; set; }

        public IActionResult OnGet()
        {
            ViewData["IdDocumentTypeId"] = new SelectList(_context.DocumentTypes.AsNoTracking(), "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? personId, Guid? customerCompanyId)
        {
            if (!ModelState.IsValid)
                return Page();

            if (personId == null)
            {
                ModelState.AddModelError("Person", "Нужно выбрать или создать Физ. лицо!");
                Person = null;
                CustomerCompany = _context.CustomerCompanies.Find(customerCompanyId);
                return Page();
            }

            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            Guid manadgerId = _context.Managers.Where(m => m.Person.ApplicationUserId == user.Id).AsNoTracking().FirstOrDefault().Id;

            Customer customerCompany = new Customer
            {
                TenantId = _tenantProvider.Tenant.Id,
                ManagerId = manadgerId,
                PersonId = personId,
                CustomerCompanyId = customerCompanyId

            };
            _context.Customers.Add(customerCompany);

            Customer customerPerson = new Customer
            {
                TenantId = _tenantProvider.Tenant.Id,
                ManagerId = manadgerId,
                PersonId = personId
            };
            _context.Customers.Add(customerPerson);  
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostCreatePersonAsync(Guid? customerCompanyId)
        {
            if (!ModelState.IsValid)
                return Page();

            Person.TenantId = _tenantProvider.Tenant.Id;
            Person.ApplicationUser.TenantId = _tenantProvider.Tenant.Id;
            _context.People.Add(Person);
            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(Person.ApplicationUser.Email))
                await _userManager.SetUserNameAsync(Person.ApplicationUser, Person.ApplicationUser.Email);
            await _userManager.AddPasswordAsync(Person.ApplicationUser, "1qaz@WSX"); // TODO: Кастыль!!! Add Password 1qaz@WSX
            await _userManager.SetLockoutEnabledAsync(Person.ApplicationUser, true);
            await _userManager.AddToRolesAsync(Person.ApplicationUser, new List<string> { "Customer" });

            CustomerCompany = _context.CustomerCompanies.Find(customerCompanyId);

            return Page();
        }

        public async Task<IActionResult> OnPostCreateCustomerCompanyAsync(Guid? personId)
        {
            if (!ModelState.IsValid)
                return Page();

            CustomerCompany.TenantId = _tenantProvider.Tenant.Id;
            _context.CustomerCompanies.Add(CustomerCompany);
            await _context.SaveChangesAsync();

            Person = _context.People.Find(personId);

            return Page();
        }
    }
}