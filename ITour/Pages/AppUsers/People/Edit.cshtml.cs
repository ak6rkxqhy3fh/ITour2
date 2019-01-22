    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;
using Microsoft.AspNetCore.Identity;
using ITour.Pages.AppUsers.Inputs;

namespace ITour.Pages.AppUsers.People
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Person Person { get; set; }

        [BindProperty]
        public InputUser InputUser { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Person = await _context.People.FirstOrDefaultAsync(m => m.Id == id);

            ApplicationUser applicationUser = await _userManager.FindByIdAsync(Person.ApplicationUserId);

            InputUser = new InputUser
            {
                Email = applicationUser.Email,
                PhoneNumber = applicationUser.PhoneNumber
            };

            if (Person == null)
            {
                return NotFound();
            }

            ViewData["IdDocumentTypeId"] = new SelectList(_context.DocumentTypes, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();  
            
            try
            {
                _context.Update(Person);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(Person.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            ApplicationUser applicationUser = await _userManager.FindByIdAsync(Person.ApplicationUserId);
            await _userManager.SetEmailAsync(applicationUser, InputUser.Email);
            await _userManager.SetUserNameAsync(applicationUser, InputUser.Email);            
            //await _userManager.SetPhoneNumberAsync(applicationUser, InputUser.PhoneNumber); // Сannot be set if email is not set
            var user = await _context.Users.FindAsync(Person.ApplicationUserId);
            user.PhoneNumber = InputUser.PhoneNumber;
            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private bool PersonExists(Guid id)
        {
            return _context.People.Any(e => e.Id == id);
        }
    }
}
