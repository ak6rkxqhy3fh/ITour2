using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;
using ITour.Pages.AppUsers.Inputs;
using Microsoft.AspNetCore.Identity;

namespace ITour.Pages.AppUsers.Customers
{
    public class EditInOrderModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditInOrderModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager; 
        }

        public Customer Customer { get; set; }

        [BindProperty]
        public Person Person { get; set; }

        [BindProperty]
        public InputUser InputUser { get; set; }

        [BindProperty]
        public CustomerCompany CustomerCompany { get; set; }


        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer = await _context.Customers
                .Include(c => c.Person)
                .Include(c => c.CustomerCompany)
                .AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (Customer == null)
            {
                return NotFound();
            }

            Person = Customer.Person;
            CustomerCompany = Customer.CustomerCompany;

            ApplicationUser applicationUser = await _userManager.FindByIdAsync(Person.ApplicationUserId);

            InputUser = new InputUser
            {
                Email = applicationUser.Email,
                PhoneNumber = applicationUser.PhoneNumber
            };

            ViewData["IdDocumentTypeId"] = new SelectList(_context.DocumentTypes.AsNoTracking(), "Id", "Name", Person?.IdDocument?.DocumentTypeId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? customerCompanyId)
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                _context.Update(Person);
                if (customerCompanyId != null)
                {
                    CustomerCompany.Id = (Guid)customerCompanyId;
                    _context.Update(CustomerCompany);
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(Customer.Id))
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

            string returnPage = (string)TempData["ReturnPage"];
            Guid orderId = (Guid)TempData["OrderId"];

            return RedirectToPage(returnPage, "", new { id = orderId }, "Customers");
        }

        private bool CustomerExists(Guid id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
