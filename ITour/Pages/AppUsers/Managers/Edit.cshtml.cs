using System;
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

namespace ITour.Pages.AppUsers.Managers
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditModel(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [BindProperty]
        public Manager Manager { get; set; }

        [BindProperty]
        [Display(Name = "Roles")]
        public List<IdentityRole> AllRoles { get; set; }

        [BindProperty]
        public List<string> ManagerRoles { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Manager = await _context.Managers.FindAsync(id);

            if (Manager == null)
            {
                return NotFound();
            }

            AllRoles = _roleManager.Roles.ToList();

            Person person = await _context.People.FirstOrDefaultAsync(p => p.Id == Manager.PersonId);
            ApplicationUser user = await _userManager.FindByIdAsync(person.ApplicationUserId);
            ManagerRoles = (List<string>)await _userManager.GetRolesAsync(user);

            ViewData["PersonId"] = new SelectList(_context.People.AsNoTracking().OrderBy(p => p.SurnameInitials), "Id", "SurnameInitials");
            ViewData["AgencyOfficeId"] = new SelectList(_context.AgencyOffices.AsNoTracking(), "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(List<string> roles)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Manager).State = EntityState.Modified;

            Person person = await _context.People.FirstOrDefaultAsync(p => p.Id == Manager.PersonId);
            ApplicationUser user = await _userManager.FindByIdAsync(person.ApplicationUserId);

            if (user != null)
            {
                            // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                            // получаем все роли
                var allRoles = _roleManager.Roles.ToList();
                            // получаем список ролей, которые были добавлены
                var addedRoles = roles.Except(userRoles);
                            // получаем роли, которые были удалены
                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ManagerExists(Manager.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ManagerExists(Guid id)
        {
            return _context.Managers.Any(e => e.Id == id);
        }
    }
}
