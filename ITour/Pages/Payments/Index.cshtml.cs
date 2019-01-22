using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;
using ITour.Utilities;

namespace ITour.Pages.Payments
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

        public List<Order> Order { get; set; }

        [BindProperty(SupportsGet = true)]
        public PaymentFilter PaymentFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public PaymentSort PaymentSort { get; set; }

        [BindProperty(SupportsGet = true)]
        public PaymentPaginate PaymentPaginate { get; set; }
        [TempData]
        public int? PaymentPageSize { get; set; }

        public PaymentTotals PaymentTotals { get; set; }

        public async Task OnGetAsync()
        {
            IQueryable<Order> orderIQ = _context.Orders
                .Include(o => o.AgencyCompany)
                .Include(o => o.Manager).ThenInclude(m => m.Person)
                .Include(o => o.Customer).ThenInclude(c => c.Person)
                .Include(o => o.Customer).ThenInclude(c => c.CustomerCompany)
                .Include(o => o.OrderStatus)
                .Include(o => o.IncomingPayments)
                .Include(o => o.OutgoingPayments).ThenInclude(op => op.PartnerCompany)
                .Include(o => o.Touroperators).ThenInclude(t => t.TouroperatorCompany)
                .Include(o => o.Services);

            orderIQ = OfficeFilterProcess(orderIQ);
            orderIQ = PaymentFilter.Process(orderIQ);
            orderIQ = PaymentSort.Process(orderIQ);
            orderIQ = PaymentPaginate.Process(orderIQ, PaymentPageSize);

            Order = await orderIQ.AsNoTracking().ToListAsync();

            PaymentTotals = new PaymentTotals(Order); // ? после пагинации?

            ViewData["FilterManagerId"] = new SelectList(_context.Managers.Include(m => m.Person).OrderBy(m => m.Person.Surname).AsNoTracking(), "Id", "Name");
            ViewData["FilterAgencyCompanyId"] = new SelectList(_context.AgencyCompanies.AsNoTracking(), "Id", "Name");            
            ViewData["PageSize"] = new SelectList(PaymentPaginate.PageSizeDictionary, "Key", "Value", PaymentPaginate.PageSize);
            PaymentPageSize = PaymentPaginate.PageSize;
            TempData.Keep("PaymentPageSize");
        }

        public IQueryable<Order> OfficeFilterProcess(IQueryable<Order> orderIQ)
        {
            if (!(User.IsInRole("Accountant") || User.IsInRole("Director") || User.IsInRole("Administrator")))
            {
                Manager manager = _context.Managers.Include(c => c.Person).ThenInclude(p => p.ApplicationUser)
                    .FirstOrDefault(m => m.Person.ApplicationUserId == _userManager.GetUserId(User));
                orderIQ = orderIQ.Where(o => o.Manager.AgencyOfficeId == manager.AgencyOfficeId);
            }
            return orderIQ;
        }
    }

    public class PaymentFilter
    {
        [Display(Name = "№")]
        public int? Number { get; set; }
        [Display(Name = "Период с"), DataType(DataType.Date)]
        public DateTime? PeriodFrom { get; set; }
        [Display(Name = "Период по"), DataType(DataType.Date)]
        public DateTime? PeriodTo { get; set; }
        [Display(Name = "Статус")]
        public Guid? StatusId { get; set; }
        [Display(Name = "Агентство")]
        public Guid? AgencyCompanyId { get; set; }
        [Display(Name = "Менеджер")]
        public Guid? ManagerId { get; set; }
        [Display(Name = "Заказчик")]
        public string CustomerName { get; set; }

        public IQueryable<Order> Process(IQueryable<Order> orderIQ)
        {
            if (Number != null)
                orderIQ = orderIQ.Where(o => o.Number == Number);

            if (PeriodFrom != null)
                orderIQ = orderIQ.Where(o => o.DatePrint >= PeriodFrom);

            if (PeriodTo != null)
                orderIQ = orderIQ.Where(o => o.DatePrint <= PeriodTo);

            if (StatusId != null)
                orderIQ = orderIQ.Where(o => o.OrderStatusId == StatusId);

            if (AgencyCompanyId != null)
                orderIQ = orderIQ.Where(o => o.AgencyCompanyId == AgencyCompanyId);

            if (ManagerId != null)
                orderIQ = orderIQ.Where(o => o.ManagerId == ManagerId);

            if (CustomerName != null)
                orderIQ = orderIQ.Where(o =>
                o.Customer.Person.Surname.Contains(CustomerName) || o.Customer.CustomerCompany.Name.Contains(CustomerName));

            return orderIQ;
        }

        public bool NotAllParamsIsNull => Number != null || PeriodFrom != null || PeriodTo != null
            || AgencyCompanyId != null || StatusId != null || ManagerId != null || CustomerName != null;
    }

    public enum PaymentSortState
    {
        OrderNumberAsc = 1, OrderNumberDesc = 2,
        AgencyCompanyNameAsc = 3, AgencyCompanyNameDesc = 4,
        ManagerNameAsc = 5, ManagerNameDesc = 6,
        CustomerNameAsc = 7, CustomerNameDesc = 8,
        OrderPrintDateAsc = 9, OrderPrintDateDesc = 10
    }

    public class PaymentSort
    {
        public PaymentSortState SortOrder { get; set; }
        public PaymentSortState SortState { get; set; }
        public PaymentSortState OrderNumberSort { get; set; }
        public PaymentSortState OrderPrintDateSort { get; set; }
        public PaymentSortState AgencyCompanyNameSort { get; set; }
        public PaymentSortState ManagerNameSort { get; set; }
        public PaymentSortState CustomerNameSort { get; set; }

        public IQueryable<Order> Process(IQueryable<Order> orderIQ)
        {
            if (SortOrder == 0)
                SortOrder = SortState;
            else
                SortState = SortOrder;

            OrderNumberSort = SortOrder == PaymentSortState.OrderNumberAsc ? PaymentSortState.OrderNumberDesc : PaymentSortState.OrderNumberAsc;
            OrderPrintDateSort = SortOrder == PaymentSortState.OrderPrintDateAsc ? PaymentSortState.OrderPrintDateDesc : PaymentSortState.OrderPrintDateAsc;
            AgencyCompanyNameSort = SortOrder == PaymentSortState.AgencyCompanyNameAsc ? PaymentSortState.AgencyCompanyNameDesc : PaymentSortState.AgencyCompanyNameAsc;
            ManagerNameSort = SortOrder == PaymentSortState.ManagerNameAsc ? PaymentSortState.ManagerNameDesc : PaymentSortState.ManagerNameAsc;
            CustomerNameSort = SortOrder == PaymentSortState.CustomerNameAsc ? PaymentSortState.CustomerNameDesc : PaymentSortState.CustomerNameAsc;

            switch (SortOrder)
            {
                case PaymentSortState.OrderNumberAsc:
                    orderIQ = orderIQ.OrderBy(o => o.Number);
                    break;

                case PaymentSortState.OrderPrintDateAsc:
                    orderIQ = orderIQ.OrderBy(o => o.DatePrint);
                    break;
                case PaymentSortState.OrderPrintDateDesc:
                    orderIQ = orderIQ.OrderByDescending(o => o.DatePrint);
                    break;

                case PaymentSortState.AgencyCompanyNameAsc:
                    orderIQ = orderIQ.OrderBy(o => o.AgencyCompany.Name);
                    break;
                case PaymentSortState.AgencyCompanyNameDesc:
                    orderIQ = orderIQ.OrderByDescending(o => o.AgencyCompany.Name);
                    break;

                case PaymentSortState.ManagerNameAsc:
                    orderIQ = orderIQ.OrderBy(o => o.Manager.Person.Surname);
                    break;
                case PaymentSortState.ManagerNameDesc:
                    orderIQ = orderIQ.OrderByDescending(o => o.Manager.Person.Surname);
                    break;

                case PaymentSortState.CustomerNameAsc:
                    orderIQ = orderIQ.OrderBy(o => o.Customer.Person.Surname);
                    break;
                case PaymentSortState.CustomerNameDesc:
                    orderIQ = orderIQ.OrderByDescending(o => o.Customer.Person.Surname);
                    break;

                default:
                    orderIQ = orderIQ.OrderByDescending(o => o.Number);
                    break;
            }
            return orderIQ;
        }
    }

    public class PaymentPaginate : Paginate<Order> { }
}
