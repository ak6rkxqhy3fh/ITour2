using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Models;
using Microsoft.AspNetCore.Identity;
using ITour.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System;

namespace ITour.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Order> Order { get; set; }

        [BindProperty(SupportsGet = true)]
        public OrderFilter OrderFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public OrderSort OrderSort { get; set; }

        [BindProperty(SupportsGet = true)]
        public OrderPaginate OrderPaginate { get; set; }
        [TempData]
        public int? OrderPageSize { get; set; }

        public List<Print> Prints { get; set; }

        public async Task OnGetAsync()
        {
            IQueryable<Order> orderIQ = _context.Orders
                .Include(o => o.AgencyCompany)
                .Include(o => o.Customer).ThenInclude(c => c.Person)
                .Include(o => o.Customer).ThenInclude(c => c.CustomerCompany)
                .Include(o => o.Manager).ThenInclude(m => m.Person)
                .Include(o => o.OrderStatus)
                .Include(o => o.TouroperatorBrand)
                .Include(o => o.IncomingPayments)
                .Include(o => o.OutgoingPayments)
                .Include(o => o.Touroperators).ThenInclude(t => t.TouroperatorCompany)
                .Include(o => o.Services).ThenInclude(s => ((AccomodationService)s).Hotel)
                .Include(o => o.Services).ThenInclude(s => ((AccomodationService)s).Resort)
                .Include(o => o.Services).ThenInclude(s => ((AccomodationService)s).Country)
                .OrderByDescending(o => o.Number);

            orderIQ = OfficeFilterProcess(orderIQ);
            orderIQ = OrderFilter.Process(orderIQ);
            orderIQ = OrderSort.Process(orderIQ);
            orderIQ = OrderPaginate.Process(orderIQ, OrderPageSize);

            Order = await orderIQ.AsNoTracking().ToListAsync();
            
            ViewData["FilterManagerId"] = new SelectList(_context.Managers.Include(m => m.Person).OrderBy(m => m.Person.Surname).AsNoTracking(), "Id", "Name");
            ViewData["FilterStatusId"] = new SelectList(_context.OrderStatuses.AsNoTracking(), "Id", "Name");
            ViewData["PageSize"] = new SelectList(OrderPaginate.PageSizeDictionary, "Key", "Value", OrderPaginate.PageSize);
            OrderPageSize = OrderPaginate.PageSize;
            TempData.Keep("OrderPageSize");

            Prints = await _context.PrintTemplates.Where(p => p.IsActive).OrderBy(p => p.Sequence)
                .Select(p => new Print
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description
                })
                .AsNoTracking().ToListAsync();
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

    public class OrderFilter
    {
        [Display(Name = "№")]
        public int? Number { get; set; }
        [Display(Name = "Период с")]
        [DataType(DataType.Date)]
        public DateTime? PeriodFrom { get; set; }
        [Display(Name = "Период по")]
        [DataType(DataType.Date)]
        public DateTime? PeriodTo { get; set; }
        [Display(Name = "Статус")]
        public Guid? StatusId { get; set; }
        [Display(Name = "Агентство")]
        public Guid? AgencyCompanyId { get; set; }
        [Display(Name = "Менеджер")]
        public Guid? ManagerId { get; set; }
        [Display(Name = "Заказчик")]
        public string CustomerName { get; set; }
        [Display(Name = "Начало тура с")]
        [DataType(DataType.Date)]
        public DateTime? DateBeginFrom { get; set; }
        [Display(Name = "Начало тура по")]
        [DataType(DataType.Date)]
        public DateTime? DateBeginTo { get; set; }


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

            if (DateBeginFrom != null)
                orderIQ = orderIQ.Where(o => o.DateBegin >= DateBeginFrom);

            if (DateBeginTo != null)
                orderIQ = orderIQ.Where(o => o.DateBegin <= DateBeginTo);

            return orderIQ;
        }
        public bool NotAllParamsIsNull => Number != null || PeriodFrom != null || PeriodTo != null
            || AgencyCompanyId != null || StatusId != null || ManagerId != null || CustomerName != null
            || DateBeginFrom != null || DateBeginTo != null;
    }

    public enum OrderSortState
    {
        OrderNumberAsc = 1, OrderNumberDesc = 2,
        AgencyCompanyNameAsc = 3, AgencyCompanyNameDesc = 4,
        ManagerNameAsc = 5, ManagerNameDesc = 6,
        CustomerNameAsc = 7, CustomerNameDesc = 8,
        OrderPrintDateAsc = 9, OrderPrintDateDesc = 10
    }

    public class OrderSort
    {
        public OrderSortState SortOrder { get; set; }
        public OrderSortState SortState { get; set; }
        public OrderSortState OrderNumberSort { get; set; }
        public OrderSortState OrderPrintDateSort { get; set; }
        public OrderSortState AgencyCompanyNameSort { get; set; }
        public OrderSortState ManagerNameSort { get; set; }
        public OrderSortState CustomerNameSort { get; set; }

        public IQueryable<Order> Process(IQueryable<Order> orderIQ)
        {
            if (SortOrder == 0)
                SortOrder = SortState;
            else
                SortState = SortOrder;

            OrderNumberSort = SortOrder == OrderSortState.OrderNumberAsc ? OrderSortState.OrderNumberDesc : OrderSortState.OrderNumberAsc;
            OrderPrintDateSort = SortOrder == OrderSortState.OrderPrintDateAsc ? OrderSortState.OrderPrintDateDesc : OrderSortState.OrderPrintDateAsc;
            AgencyCompanyNameSort = SortOrder == OrderSortState.AgencyCompanyNameAsc ? OrderSortState.AgencyCompanyNameDesc : OrderSortState.AgencyCompanyNameAsc;
            ManagerNameSort = SortOrder == OrderSortState.ManagerNameAsc ? OrderSortState.ManagerNameDesc : OrderSortState.ManagerNameAsc;
            CustomerNameSort = SortOrder == OrderSortState.CustomerNameAsc ? OrderSortState.CustomerNameDesc : OrderSortState.CustomerNameAsc;

            switch (SortOrder)
            {
                case OrderSortState.OrderNumberAsc:
                    orderIQ = orderIQ.OrderBy(o => o.Number);
                    break;

                case OrderSortState.OrderPrintDateAsc:
                    orderIQ = orderIQ.OrderBy(o => o.DatePrint);
                    break;
                case OrderSortState.OrderPrintDateDesc:
                    orderIQ = orderIQ.OrderByDescending(o => o.DatePrint);
                    break;

                case OrderSortState.AgencyCompanyNameAsc:
                    orderIQ = orderIQ.OrderBy(o => o.AgencyCompany.Name);
                    break;
                case OrderSortState.AgencyCompanyNameDesc:
                    orderIQ = orderIQ.OrderByDescending(o => o.AgencyCompany.Name);
                    break;

                case OrderSortState.ManagerNameAsc:
                    orderIQ = orderIQ.OrderBy(o => o.Manager.Person.Surname);
                    break;
                case OrderSortState.ManagerNameDesc:
                    orderIQ = orderIQ.OrderByDescending(o => o.Manager.Person.Surname);
                    break;

                case OrderSortState.CustomerNameAsc:
                    orderIQ = orderIQ.OrderBy(o => o.Customer.Person.Surname);
                    break;
                case OrderSortState.CustomerNameDesc:
                    orderIQ = orderIQ.OrderByDescending(o => o.Customer.Person.Surname);
                    break;

                default:
                    orderIQ = orderIQ.OrderByDescending(o => o.Number);
                    break;
            }
            return orderIQ;
        }
    }

    public class OrderPaginate : Paginate<Order> { }

    public class Print
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
