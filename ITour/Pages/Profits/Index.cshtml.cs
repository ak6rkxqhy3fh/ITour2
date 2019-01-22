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
using ITour.Services.Export.Profit;

namespace ITour.Pages.Profits
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
        public ProfitFilter ProfitFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public ProfitSort ProfitSort { get; set; }

        [BindProperty(SupportsGet = true)]
        public ProfitPaginate ProfitPaginate { get; set; }

        //public ProfitTotals ProfitTotals { get; set; }

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
                .Include(o => o.Services)
                .Include(o => o.Profit);

            orderIQ = OfficeFilterProcess(orderIQ);
            orderIQ = ProfitFilter.Process(orderIQ);
            orderIQ = ProfitSort.Process(orderIQ);
            orderIQ = ProfitPaginate.Process(orderIQ);

            Order = await orderIQ.AsNoTracking().ToListAsync();

            //ProfitTotals = new ProfitTotals(Order);

            ViewData["FilterManagerId"] = new SelectList(_context.Managers.Include(m => m.Person).OrderBy(m => m.Person.Surname).AsNoTracking(), "Id", "Name");
            ViewData["FilterAgencyCompanyId"] = new SelectList(_context.AgencyCompanies.AsNoTracking(), "Id", "Name");
            ViewData["PageSize"] = new SelectList(ProfitPaginate.PageSizeDictionary, "Key", "Value", ProfitPaginate.PageSize);
        }

        public IQueryable<Order> OfficeFilterProcess(IQueryable<Order> orderIQ)
        {
            if (!(User.IsInRole("Accountant") || User.IsInRole("Director") || User.IsInRole("Administrator")))
            {
                Manager manager = _context.Managers.Include(c => c.Person).ThenInclude(p => p.ApplicationUser)
                    .FirstOrDefault(m => m.Person.ApplicationUserId == _userManager.GetUserId(User));
                orderIQ = orderIQ.Where(o => o.Manager.AgencyOfficeId == manager.AgencyOfficeId);
            }

            if (User.IsInRole("NotSeeHide"))
            {
                orderIQ = orderIQ.Where(o => o.Profit.IsHide == false);
            }

            return orderIQ;
        }

        public async Task<IActionResult> OnGetExportAccountingInfoAsync(Guid orderId)
        {
            Order order = await _context.Orders.Where(o=>o.Id == orderId)
                .Include(o => o.AgencyCompany)
                .Include(o => o.Manager).ThenInclude(m => m.Person)
                .Include(o => o.Customer).ThenInclude(c => c.Person)
                .Include(o => o.Customer).ThenInclude(c => c.CustomerCompany)
                .Include(o => o.OrderStatus)
                .Include(o => o.IncomingPayments)
                .Include(o => o.OutgoingPayments).ThenInclude(op => op.PartnerCompany)
                .Include(o => o.Touroperators).ThenInclude(t => t.TouroperatorCompany)
                .Include(o => o.Services)
                .Include(o => o.Profit)
                .AsNoTracking().FirstOrDefaultAsync();

            return File(new AccountingInfo().CreatePackageAsBytes(order),
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "AccountingInfo.docx");
        }

    }

    public class ProfitFilter
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

    public enum ProfitSortState
    {
        OrderNumberAsc = 1, OrderNumberDesc = 2,
        AgencyCompanyNameAsc = 3, AgencyCompanyNameDesc = 4,
        ManagerNameAsc = 5, ManagerNameDesc = 6,
        CustomerNameAsc = 7, CustomerNameDesc = 8,
        OrderPrintDateAsc = 9, OrderPrintDateDesc = 10
    }

    public class ProfitSort
    {
        public ProfitSortState SortOrder { get; set; }
        public ProfitSortState SortState { get; set; }
        public ProfitSortState OrderNumberSort { get; set; }
        public ProfitSortState OrderPrintDateSort { get; set; }
        public ProfitSortState AgencyCompanyNameSort { get; set; }
        public ProfitSortState ManagerNameSort { get; set; }
        public ProfitSortState CustomerNameSort { get; set; }

        public IQueryable<Order> Process(IQueryable<Order> orderIQ)
        {
            if (SortOrder == 0)
                SortOrder = SortState;
            else
                SortState = SortOrder;

            OrderNumberSort = SortOrder == ProfitSortState.OrderNumberAsc ? ProfitSortState.OrderNumberDesc : ProfitSortState.OrderNumberAsc;
            OrderPrintDateSort = SortOrder == ProfitSortState.OrderPrintDateAsc ? ProfitSortState.OrderPrintDateDesc : ProfitSortState.OrderPrintDateAsc;
            AgencyCompanyNameSort = SortOrder == ProfitSortState.AgencyCompanyNameAsc ? ProfitSortState.AgencyCompanyNameDesc : ProfitSortState.AgencyCompanyNameAsc;
            ManagerNameSort = SortOrder == ProfitSortState.ManagerNameAsc ? ProfitSortState.ManagerNameDesc : ProfitSortState.ManagerNameAsc;
            CustomerNameSort = SortOrder == ProfitSortState.CustomerNameAsc ? ProfitSortState.CustomerNameDesc : ProfitSortState.CustomerNameAsc;

            switch (SortOrder)
            {
                case ProfitSortState.OrderNumberAsc:
                    orderIQ = orderIQ.OrderBy(o => o.Number);
                    break;

                case ProfitSortState.OrderPrintDateAsc:
                    orderIQ = orderIQ.OrderBy(o => o.DatePrint);
                    break;
                case ProfitSortState.OrderPrintDateDesc:
                    orderIQ = orderIQ.OrderByDescending(o => o.DatePrint);
                    break;

                case ProfitSortState.AgencyCompanyNameAsc:
                    orderIQ = orderIQ.OrderBy(o => o.AgencyCompany.Name);
                    break;
                case ProfitSortState.AgencyCompanyNameDesc:
                    orderIQ = orderIQ.OrderByDescending(o => o.AgencyCompany.Name);
                    break;

                case ProfitSortState.ManagerNameAsc:
                    orderIQ = orderIQ.OrderBy(o => o.Manager.Person.Surname);
                    break;
                case ProfitSortState.ManagerNameDesc:
                    orderIQ = orderIQ.OrderByDescending(o => o.Manager.Person.Surname);
                    break;

                case ProfitSortState.CustomerNameAsc:
                    orderIQ = orderIQ.OrderBy(o => o.Customer.Person.Surname);
                    break;
                case ProfitSortState.CustomerNameDesc:
                    orderIQ = orderIQ.OrderByDescending(o => o.Customer.Person.Surname);
                    break;

                default:
                    orderIQ = orderIQ.OrderByDescending(o => o.Number);
                    break;
            }
            return orderIQ;
        }
    }

    public class ProfitPaginate : Paginate<Order> { }
}
