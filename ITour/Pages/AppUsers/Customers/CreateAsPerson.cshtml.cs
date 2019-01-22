using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ITour.Data;
using ITour.Models;
using ITour.Services.Tenants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ITour.Pages.AppUsers.Customers
{
    public class CreateAsPersonModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ITenantProvider _tenantProvider;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateAsPersonModel(ApplicationDbContext context, ITenantProvider tenantProvider, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _tenantProvider = tenantProvider;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Person Person { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Person.TenantId = _tenantProvider.Tenant.Id;
            Person.ApplicationUser.TenantId = _tenantProvider.Tenant.Id;
            _context.People.Add(Person);
            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(Person.ApplicationUser.Email))
                await _userManager.SetUserNameAsync(Person.ApplicationUser, Person.ApplicationUser.Email);
            await _userManager.AddPasswordAsync(Person.ApplicationUser, "1qaz@WSX"); // TODO: Кастыль!!! Add Password 1qaz@WSX
            await _userManager.SetLockoutEnabledAsync(Person.ApplicationUser, true);
            await _userManager.AddToRolesAsync(Person.ApplicationUser, new List<string> { "Customer" });

            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            Guid manadgerId = _context.Managers.Where(m => m.Person.ApplicationUserId == user.Id).AsNoTracking().FirstOrDefault().Id;

            Customer customerPerson = new Customer
            {
                TenantId = _tenantProvider.Tenant.Id,
                ManagerId = manadgerId,
                PersonId = Person.Id
            };
            _context.Customers.Add(customerPerson);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}