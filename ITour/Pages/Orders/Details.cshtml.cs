using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = await _context.Orders
                .Include(o => o.AgencyCompany)
                .Include(o => o.Customer).ThenInclude(c => c.Person)
                .Include(o => o.Customer).ThenInclude(c => c.CustomerCompany)
                .Include(o => o.Manager).ThenInclude(m => m.Person)
                .Include(o => o.Manager).ThenInclude(m => m.AgencyOffice)
                .Include(o => o.OrderStatus)
                .Include(o => o.TouroperatorBrand)
                .Include(o => o.Touroperators).ThenInclude(t => t.TouroperatorCompany).ThenInclude(tc => tc.TouroperatorBrands).ThenInclude(tb => tb.TouroperatorBrand)
                .Include(o => o.Services).ThenInclude(s => ((AccomodationService)s).Hotel)
                .Include(o => o.Services).ThenInclude(s => ((AccomodationService)s).Resort)
                .Include(o => o.Services).ThenInclude(s => ((AccomodationService)s).Country)
                .Include(o => o.Services).ThenInclude(s => ((AccomodationService)s).FoodType)
                .Include(o => o.Services).ThenInclude(s => ((AccomodationService)s).RoomType)
                .Include(o => o.Services).ThenInclude(s => ((TransportService)s).TransportType)
                .Include(o => o.Services).ThenInclude(s => ((TransferService)s).TransferType)
                .Include(o => o.Services).ThenInclude(s => ((InsuranceService)s).InsuranceCompany)
                .Include(o => o.Services).ThenInclude(s => ((InsuranceService)s).InsuranceType)
                .Include(o => o.Services).ThenInclude(s => ((VisaService)s).VisaType)
                .Include(o => o.IncomingPayments)
                .Include(o => o.OutgoingPayments)
                .AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (Order == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
