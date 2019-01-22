using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ITour.Data;
using ITour.Models;
using Microsoft.AspNetCore.Identity;
using ITour.Services.Tenants;

namespace ITour.Pages.AppUsers.People
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ITenantProvider _tenantProvider;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(ApplicationDbContext context, ITenantProvider tenantProvider, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _tenantProvider = tenantProvider;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            ViewData["IdDocumentTypeId"] = new SelectList(_context.DocumentTypes, "Id", "Name");

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

            Guid? tenantId =  _tenantProvider.Tenant.Id;
            Person.TenantId = tenantId;
            Person.ApplicationUser.TenantId = tenantId;
            _context.People.Add(Person);
            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(Person.ApplicationUser.Email))
                await _userManager.SetUserNameAsync(Person.ApplicationUser, Person.ApplicationUser.Email);
            await _userManager.AddPasswordAsync(Person.ApplicationUser, "1qaz@WSX"); // TODO: Кастыль!!! Add Password 1qaz@WSX
            await _userManager.SetLockoutEnabledAsync(Person.ApplicationUser, true);
            await _userManager.AddToRolesAsync(Person.ApplicationUser, new List<string> { "Customer" });           

            return RedirectToPage("./Index");
        }
    }
}