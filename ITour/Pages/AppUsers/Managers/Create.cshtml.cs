using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;
using ITour.Services.Tenants;

namespace ITour.Pages.AppUsers.Managers
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ITenantProvider _tenantProvider;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(ApplicationDbContext context, ITenantProvider tenantProvider, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _tenantProvider = tenantProvider;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [BindProperty]
        public Manager Manager { get; set; }

        [BindProperty]
        [Display(Name = "Роли")]
        public List<IdentityRole> AllRoles { get; set; }

        [BindProperty]
        public List<string> ManagerRoles { get; set; }

        public IActionResult OnGet()
        {

            AllRoles = _roleManager.Roles.ToList();

            ViewData["PersonId"] = new SelectList(_context.People.Where(p => p.IsEmployee).AsNoTracking().OrderBy(p => p.SurnameInitials), "Id", "SurnameInitials");
            ViewData["AgencyOfficeId"] = new SelectList(_context.AgencyOffices.AsNoTracking(), "Id", "Name");
            return Page();
        }


        public async Task<IActionResult> OnPostAsync(List<string> roles)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Manager.TenantId = _tenantProvider.Tenant.Id;
            _context.Managers.Add(Manager);

            Person person = await _context.People.FirstOrDefaultAsync(p => p.Id == Manager.PersonId);
            ApplicationUser user = await _userManager.FindByIdAsync(person.ApplicationUserId);
            if (user != null)
            {
                await _userManager.AddToRolesAsync(user, roles);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}