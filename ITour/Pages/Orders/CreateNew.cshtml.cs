using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ITour.Data;
using ITour.Models;
using ITour.Services.Tenants;
using System;

namespace ITour.Pages.Orders
{
    public class CreateCreateNewModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ITenantProvider _tenantProvider;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateCreateNewModel(ApplicationDbContext context, ITenantProvider tenantProvider, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _tenantProvider = tenantProvider;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            Guid? managerId = null;
            string userId = _userManager.GetUserId(User);
            if (!string.IsNullOrEmpty(userId))
            {
                managerId = _context.Managers.Include(c => c.Person).AsNoTracking()
                    .FirstOrDefault(m => m.Person.ApplicationUserId == userId).Id;
            }

            Order Order = new Order
            {
                TenantId = _tenantProvider.Tenant.Id,
                ManagerId = managerId
            };
            _context.Orders.Add(Order);

            Profit profit = new Profit
            {
                OrderId = Order.Id,
                TenantId = Order.TenantId
            };
            _context.Profits.Add(profit);

            _context.SaveChanges();

            return RedirectToPage("./Edit", new { Order.Id });
        }
    }
}
