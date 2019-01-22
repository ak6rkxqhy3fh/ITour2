using ITour.Data;
using ITour.Models;
using ITour.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITour.Pages.Reports.IncomingPayments
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

        public IList<IncomingPayment> IncomingPayment { get; set; }

        [BindProperty(SupportsGet = true)]
        public IncomingPaymentFilter IncomingPaymentFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public IncomingPaymentSort IncomingPaymentSort { get; set; }

        [BindProperty(SupportsGet = true)]
        public IncomingPaymentPaginate IncomingPaymentPaginate { get; set; }

        public IncomingPaymentTotals IncomingPaymentTotals { get; set; }


        public async Task OnGetAsync()
        {
            IQueryable<IncomingPayment> incomingPaymentIQ = _context.IncomingPayments
                .Include(i => i.Order).ThenInclude(o => o.AgencyCompany)
                .Include(i => i.Order).ThenInclude(o => o.Manager).ThenInclude(m => m.AgencyOffice)
                .Include(i => i.Order).ThenInclude(o => o.Manager).ThenInclude(m => m.Person)
                .Include(i => i.Order).ThenInclude(o => o.Customer).ThenInclude(c => c.Person)
                .Include(i => i.Order).ThenInclude(o => o.Customer).ThenInclude(c => c.CustomerCompany)
                .Include(i => i.Order).ThenInclude(o => o.Touroperators).ThenInclude(c => c.TouroperatorCompany)
                .Include(i => i.PaymentForm)
                .Include(i => i.PaymentType);

            incomingPaymentIQ = OfficeFilterProcess(incomingPaymentIQ);
            incomingPaymentIQ = IncomingPaymentFilter.Process(incomingPaymentIQ);
            incomingPaymentIQ = IncomingPaymentSort.Process(incomingPaymentIQ);

            if (IncomingPaymentPaginate.PageSize == 0)
                IncomingPaymentPaginate.PageSize = 50;

            if (IncomingPaymentFilter.AllParamsIsNull || (IncomingPaymentFilter.NotAllParamsIsNull && incomingPaymentIQ.Count() > 300))
            {
                incomingPaymentIQ = IncomingPaymentPaginate.Process(incomingPaymentIQ);
            }

            IncomingPayment = await incomingPaymentIQ.AsNoTracking().ToListAsync();

            IncomingPaymentTotals = new IncomingPaymentTotals(IncomingPayment); // TODO: PaymentTotals после пагинации???

            ViewData["FilterManagerId"] = new SelectList(_context.Managers.Include(m => m.Person).OrderBy(m => m.Person.Surname).AsNoTracking(), "Id", "Name");
            ViewData["FilterAgencyCompanyId"] = new SelectList(_context.AgencyCompanies.AsNoTracking(), "Id", "Name");
            ViewData["FilterAgencyOfficeId"] = new SelectList(_context.AgencyOffices.AsNoTracking(), "Id", "Name");
            ViewData["PageSize"] = new SelectList(IncomingPaymentPaginate.PageSizeDictionary, "Key", "Value", IncomingPaymentPaginate.PageSize);
        }
        public IQueryable<IncomingPayment> OfficeFilterProcess(IQueryable<IncomingPayment> incomingPaymentIQ)
        {
            if (!(User.IsInRole("Accountant") || User.IsInRole("Director") || User.IsInRole("Administrator")))
            {
                Manager manager = _context.Managers.Include(c => c.Person).ThenInclude(p => p.ApplicationUser)
                    .FirstOrDefault(m => m.Person.ApplicationUserId == _userManager.GetUserId(User));
                incomingPaymentIQ = incomingPaymentIQ.Where(o => o.Order.Manager.AgencyOfficeId == manager.AgencyOfficeId);
            }
            return incomingPaymentIQ;
        }
    }

    public class IncomingPaymentFilter
    {
        [Display(Name = "№")]
        public int? OrderNumber { get; set; }
        [Display(Name = "Период с"), DataType(DataType.Date)]
        public DateTime? PeriodFrom { get; set; }
        [Display(Name = "Период по"), DataType(DataType.Date)]
        public DateTime? PeriodTo { get; set; }
        [Display(Name = "Агентство")]
        public Guid? AgencyCompanyId { get; set; }
        [Display(Name = "Офис")]
        public Guid? AgencyOfficeId { get; set; }
        [Display(Name = "Менеджер")]
        public Guid? ManagerId { get; set; }
        [Display(Name = "Заказчик")]
        public string CustomerName { get; set; }

        public IQueryable<IncomingPayment> Process(IQueryable<IncomingPayment> incomingPaymentIQ)
        {
            if (OrderNumber != null)
                incomingPaymentIQ = incomingPaymentIQ.Where(o => o.Order.Number == OrderNumber);

            if (PeriodFrom != null)
                incomingPaymentIQ = incomingPaymentIQ.Where(o => o.PaymentDate >= PeriodFrom);

            if (PeriodTo != null)
                incomingPaymentIQ = incomingPaymentIQ.Where(o => o.PaymentDate <= PeriodTo);

            if (AgencyCompanyId != null)
                incomingPaymentIQ = incomingPaymentIQ.Where(o => o.Order.AgencyCompanyId == AgencyCompanyId);

            if (AgencyOfficeId != null)
                incomingPaymentIQ = incomingPaymentIQ.Where(o => o.Order.Manager.AgencyOfficeId == AgencyOfficeId);

            if (ManagerId != null)
                incomingPaymentIQ = incomingPaymentIQ.Where(o => o.Order.ManagerId == ManagerId);

            if (CustomerName != null)
                incomingPaymentIQ = incomingPaymentIQ.Where(o =>
                o.Order.Customer.Person.Surname.Contains(CustomerName) || o.Order.Customer.CustomerCompany.Name.Contains(CustomerName));

            return incomingPaymentIQ;
        }

        public bool NotAllParamsIsNull => OrderNumber != null || PeriodFrom != null || PeriodTo != null
            || AgencyCompanyId != null || AgencyOfficeId != null || ManagerId != null || CustomerName != null;

        public bool AllParamsIsNull => !NotAllParamsIsNull;
    }

    public enum IncomingPaymentSortState
    {
        OrderNumberAsc = 1, OrderNumberDesc = 2,
        AgencyCompanyNameAsc = 3, AgencyCompanyNameDesc = 4,
        AgencyOfficeNameAsc = 5, AgencyOfficeNameDesc = 6,
        ManagerNameAsc = 7, ManagerNameDesc = 8,
        CustomerNameAsc = 9, CustomerNameDesc = 10,
        PaymentDateAsc = 11, PaymentDateDesc = 12
    }

    public class IncomingPaymentSort
    {
        public IncomingPaymentSortState SortOrder { get; set; }
        public IncomingPaymentSortState SortState { get; set; }
        public IncomingPaymentSortState OrderNumberSort { get; set; }
        public IncomingPaymentSortState PaymentDateSort { get; set; }
        public IncomingPaymentSortState AgencyCompanyNameSort { get; set; }
        public IncomingPaymentSortState AgencyOfficeNameSort { get; set; }
        public IncomingPaymentSortState ManagerNameSort { get; set; }
        public IncomingPaymentSortState CustomerNameSort { get; set; }

        public IQueryable<IncomingPayment> Process(IQueryable<IncomingPayment> incomingPaymentIQ)
        {
            if (SortOrder == 0)
                SortOrder = SortState;
            else
                SortState = SortOrder;

            OrderNumberSort = SortOrder == IncomingPaymentSortState.OrderNumberAsc ? IncomingPaymentSortState.OrderNumberDesc : IncomingPaymentSortState.OrderNumberAsc;
            PaymentDateSort = SortOrder == IncomingPaymentSortState.PaymentDateAsc ? IncomingPaymentSortState.PaymentDateDesc : IncomingPaymentSortState.PaymentDateAsc;
            AgencyCompanyNameSort = SortOrder == IncomingPaymentSortState.AgencyCompanyNameAsc ? IncomingPaymentSortState.AgencyCompanyNameDesc : IncomingPaymentSortState.AgencyCompanyNameAsc;
            AgencyOfficeNameSort = SortOrder == IncomingPaymentSortState.AgencyOfficeNameAsc ? IncomingPaymentSortState.AgencyOfficeNameDesc : IncomingPaymentSortState.AgencyOfficeNameAsc;
            ManagerNameSort = SortOrder == IncomingPaymentSortState.ManagerNameAsc ? IncomingPaymentSortState.ManagerNameDesc : IncomingPaymentSortState.ManagerNameAsc;
            CustomerNameSort = SortOrder == IncomingPaymentSortState.CustomerNameAsc ? IncomingPaymentSortState.CustomerNameDesc : IncomingPaymentSortState.CustomerNameAsc;

            switch (SortOrder)
            {
                case IncomingPaymentSortState.PaymentDateAsc:
                    incomingPaymentIQ = incomingPaymentIQ.OrderBy(o => o.PaymentDate);
                    break;

                case IncomingPaymentSortState.OrderNumberAsc:
                    incomingPaymentIQ = incomingPaymentIQ.OrderBy(o => o.Order.Number);
                    break;

                case IncomingPaymentSortState.OrderNumberDesc:
                    incomingPaymentIQ = incomingPaymentIQ.OrderByDescending(o => o.Order.Number);
                    break;

                case IncomingPaymentSortState.AgencyCompanyNameAsc:
                    incomingPaymentIQ = incomingPaymentIQ.OrderBy(o => o.Order.AgencyCompany.Name);
                    break;
                case IncomingPaymentSortState.AgencyCompanyNameDesc:
                    incomingPaymentIQ = incomingPaymentIQ.OrderByDescending(o => o.Order.AgencyCompany.Name);
                    break;

                case IncomingPaymentSortState.AgencyOfficeNameAsc:
                    incomingPaymentIQ = incomingPaymentIQ.OrderBy(o => o.Order.Manager.AgencyOffice.Name);
                    break;

                case IncomingPaymentSortState.AgencyOfficeNameDesc:
                    incomingPaymentIQ = incomingPaymentIQ.OrderByDescending(o => o.Order.Manager.AgencyOffice.Name);
                    break;

                case IncomingPaymentSortState.ManagerNameAsc:
                    incomingPaymentIQ = incomingPaymentIQ.OrderBy(o => o.Order.Manager.Person.Surname);
                    break;
                case IncomingPaymentSortState.ManagerNameDesc:
                    incomingPaymentIQ = incomingPaymentIQ.OrderByDescending(o => o.Order.Manager.Person.Surname);
                    break;

                case IncomingPaymentSortState.CustomerNameAsc:
                    incomingPaymentIQ = incomingPaymentIQ.OrderBy(o => o.Order.Customer.Person.Surname);
                    break;
                case IncomingPaymentSortState.CustomerNameDesc:
                    incomingPaymentIQ = incomingPaymentIQ.OrderByDescending(o => o.Order.Customer.Person.Surname);
                    break;

                default:
                    incomingPaymentIQ = incomingPaymentIQ.OrderByDescending(o => o.PaymentDate);
                    break;
            }
            return incomingPaymentIQ;
        }
    }

    public class IncomingPaymentPaginate : Paginate<IncomingPayment> { }

    public class IncomingPaymentTotals
    {
        public IncomingPaymentTotals(IList<IncomingPayment> incomingPayments)
        {
            TotalPaymentsAmount = incomingPayments.Sum(ip => (decimal)ip.PaymentAmount);
            TotalBankComission = incomingPayments.Sum(ip => (decimal)ip.BankCommission);
        }

        public decimal TotalPaymentsAmount { get; set; }
        [Display(Name = "Всего Входящие платежи")]
        public string TotalPaymentAmountCurrency => $"{TotalPaymentsAmount.ToString("C")}";
        [Display(Name = "Всего Входящие платежи")]
        public string TotalPaymentAmountNumeric => $"{TotalPaymentsAmount.ToString("N")}";

        public decimal TotalBankComission { get; set; }
        [Display(Name = "Всего комиссия")]
        public string TotalBankComissionCurency => $"{TotalBankComission.ToString("C")}";
        [Display(Name = "Всего комиссия")]
        public string TotalBankComissionNumeric => $"{TotalBankComission.ToString("N")}";
    }
}
