using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;
using ITour.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITour.Pages.Reports.ComingDeparture
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        public List<TransportService> TransportService { get; set; }

        [BindProperty(SupportsGet = true)]
        public ServiceFilter<TransportService> TransportServiceFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public TransportServiceSort TransportServiceSort { get; set; }

        [BindProperty(SupportsGet = true)]
        public TransportServicePaginate TransportServicePaginate { get; set; }

        public async Task OnGetAsync()
        {
            IQueryable<TransportService> transportServiceIQ = _context.TransportServices
                .Include(t => t.Order).ThenInclude(o => o.Manager).ThenInclude(m => m.AgencyOffice)
                .Include(t => t.Order).ThenInclude(o => o.Manager).ThenInclude(m => m.Person)
                .Include(t => t.Order).ThenInclude(o => o.Customer).ThenInclude(с => с.Person)
                .Include(t => t.Order).ThenInclude(o => o.Customer).ThenInclude(с => с.CustomerCompany)
                .Include(t => t.TransportType)
                .Where(t => t.DateBegin != null)
                .GroupBy(t => t.OrderId)
                .Select(tg => tg.OrderBy(t => t.DateBegin).FirstOrDefault());

            transportServiceIQ = TransportServiceFilter.Process(transportServiceIQ);
            transportServiceIQ = TransportServiceSort.Process(transportServiceIQ);
            transportServiceIQ = TransportServicePaginate.Process(transportServiceIQ);

            TransportService = await transportServiceIQ.ToListAsync();

            ViewData["FilterManagerId"] = new SelectList(_context.Managers.Include(m => m.Person).OrderBy(m => m.Person.Surname).AsNoTracking(), "Id", "Name");
            ViewData["PageSize"] = new SelectList(TransportServicePaginate.PageSizeDictionary, "Key", "Value", TransportServicePaginate.PageSize);
        }
    }
    public enum TransportServiceSortState
    {
        OrderNumberAsc = 1, OrderNumberDesc = 2,
        DepartureDateAsc = 3, DepartureDateDesc = 4,
        ManagerNameAsc = 5, ManagerNameDesc = 6,
        CustomerNameAsc = 7, CustomerNameDesc = 8
    }

    public class TransportServiceSort
    {
        public TransportServiceSortState SortOrder { get; set; }
        public TransportServiceSortState SortState { get; set; }
        public TransportServiceSortState OrderNumberSort { get; set; }
        public TransportServiceSortState DepartureDateSort { get; set; }
        public TransportServiceSortState ManagerNameSort { get; set; }
        public TransportServiceSortState CustomerNameSort { get; set; }

        public IQueryable<TransportService> Process(IQueryable<TransportService> transportServiceIQ)
        {

            if (SortOrder == 0)
                SortOrder = SortState;
            else
                SortState = SortOrder;

            OrderNumberSort = SortOrder == TransportServiceSortState.OrderNumberAsc ? TransportServiceSortState.OrderNumberDesc : TransportServiceSortState.OrderNumberAsc;
            DepartureDateSort = SortOrder == TransportServiceSortState.DepartureDateAsc ? TransportServiceSortState.DepartureDateDesc : TransportServiceSortState.DepartureDateAsc;
            ManagerNameSort = SortOrder == TransportServiceSortState.ManagerNameAsc ? TransportServiceSortState.ManagerNameDesc : TransportServiceSortState.ManagerNameAsc;
            CustomerNameSort = SortOrder == TransportServiceSortState.CustomerNameAsc ? TransportServiceSortState.CustomerNameDesc : TransportServiceSortState.CustomerNameAsc;

            switch (SortOrder)
            {
                case TransportServiceSortState.DepartureDateDesc:
                    transportServiceIQ = transportServiceIQ.OrderByDescending(c => c.DateBegin);
                    break;

                case TransportServiceSortState.OrderNumberAsc:
                    transportServiceIQ = transportServiceIQ.OrderBy(c => c.Order.Number);
                    break;
                case TransportServiceSortState.OrderNumberDesc:
                    transportServiceIQ = transportServiceIQ.OrderByDescending(c => c.Order.Number);
                    break;                

                case TransportServiceSortState.ManagerNameAsc:
                    transportServiceIQ = transportServiceIQ.OrderBy(c => c.Order.Manager.Name);
                    break;
                case TransportServiceSortState.ManagerNameDesc:
                    transportServiceIQ = transportServiceIQ.OrderByDescending(c => c.Order.Manager.Person.Surname);
                    break;

                case TransportServiceSortState.CustomerNameAsc:
                    transportServiceIQ = transportServiceIQ.OrderBy(c => c.Order.Customer.Person.Surname);
                    break;
                case TransportServiceSortState.CustomerNameDesc:
                    transportServiceIQ = transportServiceIQ.OrderByDescending(c => c.Order.Customer.Person.Surname);
                    break;

                default:
                    transportServiceIQ = transportServiceIQ.OrderBy(c => c.DateBegin);
                    break;
            }
            return transportServiceIQ;
        }
    }

    public class TransportServicePaginate : Paginate<TransportService> { }
}
