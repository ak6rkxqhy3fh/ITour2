using ITour.Data;
using ITour.Models;
using ITour.Services.Export.Commission;
using ITour.Services.Tenants;
using ITour.Utilities;
using Microsoft.AspNetCore.Hosting;
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

namespace ITour.Pages.Reports.Commissions
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ITenantProvider _tenantProvider;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        public IndexModel(ApplicationDbContext context, ITenantProvider tenantProvider, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _tenantProvider = tenantProvider;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        public List<Commission> Commission { get; set; }

        public CommissionTotals CommissionTotals { get; set; }

        [BindProperty(SupportsGet = true)]
        public CommissionFilter CommissionFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public CommissionSort CommissionSort { get; set; }

        [BindProperty(SupportsGet = true)]
        public CommissionPaginate CommissionPaginate { get; set; }

        public async Task OnGetAsync()
        {
            IQueryable<Commission> commissionIQ = _context.Commission.FromSql(Models.Commission.CommissionQuery); // TODO Переделать -> LINQ

            commissionIQ = OfficeFilterProcess(commissionIQ);
            commissionIQ = CommissionFilter.Process(commissionIQ);
            commissionIQ = CommissionSort.Process(commissionIQ);

            if (CommissionFilter.AllParamsIsNull || (CommissionFilter.NotAllParamsIsNull && commissionIQ.Count() > 300))
            {
                commissionIQ = CommissionPaginate.Process(commissionIQ);
            }

            Commission = await commissionIQ.AsNoTracking().ToListAsync();

            await SetCommissionTouroperators(Commission); // TODO Кастыль!
            await SetCommissionPartners(Commission); // TODO Кастыль!

            CommissionTotals = new CommissionTotals(Commission);

            ViewData["FilterAgencyCompanyId"] = new SelectList(_context.AgencyCompanies.AsNoTracking(), "Id", "Name");
            ViewData["FilterManagerId"] = new SelectList(_context.Managers.Include(m => m.Person).OrderBy(m => m.Person.Surname).AsNoTracking(), "Id", "Name");
            ViewData["PageSize"] = new SelectList(CommissionPaginate.PageSizeDictionary, "Key", "Value", CommissionPaginate.PageSize);
        }

        public IQueryable<Commission> OfficeFilterProcess(IQueryable<Commission> commissionIQ)
        {
            if (!(User.IsInRole("Accountant") || User.IsInRole("Director") || User.IsInRole("Administrator")))
            {
                Manager manager = _context.Managers.Include(c => c.Person).ThenInclude(p => p.ApplicationUser)
                    .FirstOrDefault(m => m.Person.ApplicationUserId == _userManager.GetUserId(User));
                commissionIQ = commissionIQ.Where(c => c.AgencyOfficeId == manager.AgencyOfficeId);
            }
            return commissionIQ;
        }

        public async Task<IActionResult> OnPostExportCommissionReportAsync()
        {
            IQueryable<Commission> commissionIQ = _context.Commission.FromSql(Models.Commission.CommissionQuery);

            commissionIQ = OfficeFilterProcess(commissionIQ);
            commissionIQ = CommissionFilter.Process(commissionIQ);
            commissionIQ = CommissionSort.Process(commissionIQ);

            Commission = await commissionIQ.AsNoTracking().ToListAsync();

            await SetCommissionTouroperators(Commission); // TODO Кастыль!
            await SetCommissionPartners(Commission); // TODO Кастыль!

            return File(new CommissionReport().CreateReportAsBytes(Commission),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"CommissionReport.xlsx");
        }

        private async Task SetCommissionTouroperators(List<Commission> Commission)
        {
            foreach (Commission commission in Commission)
            {
                List<OrderTouroperatorCompany> touroperatorCompanies = await _context.OrderTouroperatorCompanies
                    .Include(otc => otc.TouroperatorCompany)
                    .Where(otc => otc.OrderId == commission.OrderId)
                    .AsNoTracking().ToListAsync();

                string touroperators = "";
                foreach (OrderTouroperatorCompany touroperator in touroperatorCompanies)
                {
                    touroperators += $"{touroperator.TouroperatorCompany?.Name} ";
                }

                commission.Touroperators = !string.IsNullOrEmpty(touroperators) ? touroperators : "";
            }
        }

        private async Task SetCommissionPartners(List<Commission> Commission)
        {
            foreach (Commission commission in Commission)
            {
                List<OutgoingPayment> payments = await _context.OutgoingPayments.Include(pc => pc.PartnerCompany)
                    .Where(op => op.OrderId == commission.OrderId)
                    .AsNoTracking().ToListAsync();

                string partners = "";
                foreach (OutgoingPayment payment in payments)
                {
                    partners += $"{payment.PartnerCompany?.Name} ";
                }

                commission.Partners = !string.IsNullOrEmpty(partners) ? partners : "";
            }
        }
    }
}

public class CommissionFilter
{
    [Display(Name = "Заказ")]
    public int? OrderNumber { get; set; }
    [Display(Name = "Агентство")]
    public Guid? AgencyCompanyId { get; set; }
    [Display(Name = "Менеджер")]
    public Guid? ManagerId { get; set; }
    [Display(Name = "Заказчик")]
    public string CustomerName { get; set; }
    [Display(Name = "Дата комиссии с")]
    [DataType(DataType.Date)]
    public DateTime? OrderCommissionDateFrom { get; set; }
    [Display(Name = "Дата комиссии по")]
    [DataType(DataType.Date)]
    public DateTime? OrderCommissionDateTo { get; set; }

    public IQueryable<Commission> Process(IQueryable<Commission> commissionIQ)
    {
        if (OrderNumber != null)
            commissionIQ = commissionIQ.Where(c => c.OrderNumber == OrderNumber);

        if (AgencyCompanyId != null)
            commissionIQ = commissionIQ.Where(c => c.AgencyCompanyId == AgencyCompanyId);

        if (ManagerId != null)
            commissionIQ = commissionIQ.Where(c => c.ManagerId == ManagerId);

        if (CustomerName != null)
            commissionIQ = commissionIQ.Where(c =>
            c.CustomerName.Contains(CustomerName) || c.CustomerCompanyName.Contains(CustomerName));

        if (OrderCommissionDateFrom != null)
            commissionIQ = commissionIQ.Where(c => c.OrderCommissionDate >= OrderCommissionDateFrom);

        if (OrderCommissionDateTo != null)
            commissionIQ = commissionIQ.Where(c => c.OrderCommissionDate <= OrderCommissionDateTo);

        return commissionIQ;
    }

    public bool NotAllParamsIsNull => OrderNumber != null || AgencyCompanyId != null || ManagerId != null
        || CustomerName != null || OrderCommissionDateFrom != null || OrderCommissionDateTo != null;

    public bool AllParamsIsNull => !NotAllParamsIsNull;
}

public enum CommissionSortState
{
    OrderNumberAsc = 1, OrderNumberDesc = 2,
    AgencyCompanyNameAsc = 3, AgencyCompanyNameDesc = 4,
    ManagerNameAsc = 5, ManagerNameDesc = 6,
    CustomerNameAsc = 7, CustomerNameDesc = 8,
    OrderCommissionDateAsc = 9, OrderCommissionDateDesc = 10
}

public class CommissionSort
{
    public CommissionSortState SortOrder { get; set; }
    public CommissionSortState SortState { get; set; }
    public CommissionSortState OrderNumberSort { get; set; }
    public CommissionSortState AgencyCompanyNameSort { get; set; }
    public CommissionSortState ManagerNameSort { get; set; }
    public CommissionSortState CustomerNameSort { get; set; }
    public CommissionSortState OrderCommissionDateSort { get; set; }

    public IQueryable<Commission> Process(IQueryable<Commission> commissionIQ)
    {

        if (SortOrder == 0)
            SortOrder = SortState;
        else
            SortState = SortOrder;

        OrderNumberSort = SortOrder == CommissionSortState.OrderNumberAsc ? CommissionSortState.OrderNumberDesc : CommissionSortState.OrderNumberAsc;
        AgencyCompanyNameSort = SortOrder == CommissionSortState.AgencyCompanyNameAsc ? CommissionSortState.AgencyCompanyNameDesc : CommissionSortState.AgencyCompanyNameAsc;
        ManagerNameSort = SortOrder == CommissionSortState.ManagerNameAsc ? CommissionSortState.ManagerNameDesc : CommissionSortState.ManagerNameAsc;
        CustomerNameSort = SortOrder == CommissionSortState.CustomerNameAsc ? CommissionSortState.CustomerNameDesc : CommissionSortState.CustomerNameAsc;
        OrderCommissionDateSort = SortOrder == CommissionSortState.OrderCommissionDateAsc ? CommissionSortState.OrderCommissionDateDesc : CommissionSortState.OrderCommissionDateAsc;

        switch (SortOrder)
        {
            case CommissionSortState.OrderNumberDesc:
                commissionIQ = commissionIQ.OrderByDescending(c => c.OrderNumber);
                break;

            case CommissionSortState.AgencyCompanyNameAsc:
                commissionIQ = commissionIQ.OrderBy(c => c.AgencyCompanyName);
                break;
            case CommissionSortState.AgencyCompanyNameDesc:
                commissionIQ = commissionIQ.OrderByDescending(c => c.AgencyCompanyName);
                break;

            case CommissionSortState.ManagerNameAsc:
                commissionIQ = commissionIQ.OrderBy(c => c.ManagerName);
                break;
            case CommissionSortState.ManagerNameDesc:
                commissionIQ = commissionIQ.OrderByDescending(c => c.ManagerName);
                break;

            case CommissionSortState.CustomerNameAsc:
                commissionIQ = commissionIQ.OrderBy(c => c.CustomerName);
                break;
            case CommissionSortState.CustomerNameDesc:
                commissionIQ = commissionIQ.OrderByDescending(c => c.CustomerName);
                break;

            case CommissionSortState.OrderCommissionDateAsc:
                commissionIQ = commissionIQ.OrderBy(c => c.OrderCommissionDate);
                break;
            case CommissionSortState.OrderCommissionDateDesc:
                commissionIQ = commissionIQ.OrderByDescending(c => c.OrderCommissionDate);
                break;

            default:
                commissionIQ = commissionIQ.OrderBy(c => c.OrderNumber);
                break;
        }
        return commissionIQ;
    }
}

public class CommissionPaginate : Paginate<Commission> { }