using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ITour.Data;
using ITour.Models;
using ITour.Services.Tenants;
using System.Collections.Generic;

namespace ITour.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ITenantProvider _tenantProvider;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(ApplicationDbContext context, ITenantProvider tenantProvider, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _tenantProvider = tenantProvider;
            _userManager = userManager;
        }

        [BindProperty]
        public Order Order { get; set; }

        [TempData]
        public Guid OrderId { get; set; }

        [TempData]
        public string ReturnPage { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public List<Print> Prints { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                string userId = _userManager.GetUserId(User);
                Manager manager = await _context.Managers.Include(c => c.Person).AsNoTracking()
                        .FirstOrDefaultAsync(m => m.Person.ApplicationUserId == userId);

                Order = new Order
                {
                    TenantId = _tenantProvider.Tenant.Id,
                    ManagerId = manager.Id
                };

                _context.Orders.Add(Order);

                Profit profit = new Profit
                {
                    OrderId = Order.Id,
                    TenantId = Order.TenantId
                };
                _context.Profits.Add(profit);

                _context.SaveChanges();
            }
            else
            {
                Order = await _context.Orders
                    .Include(o => o.AgencyCompany)
                    .Include(o => o.Customer).ThenInclude(c => c.Person)
                    .Include(o => o.Customer).ThenInclude(c => c.CustomerCompany)
                    .Include(o => o.Manager).ThenInclude(m => m.Person)
                    .Include(o => o.Manager).ThenInclude(m => m.AgencyOffice)
                    .Include(o => o.OrderStatus)
                    .Include(o => o.Touroperators).ThenInclude(t => t.TouroperatorCompany)
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
                    .IgnoreQueryFilters().FirstOrDefaultAsync(m => m.Id == id);

                if (Order == null || Order.TenantId != _tenantProvider.Tenant.Id || Order.IsDeleted)
                    return NotFound();
            }

            Prints = await _context.PrintTemplates.Where(p => p.IsActive).OrderBy(p => p.Sequence)
                .Select(p => new Print
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description
                })
                .AsNoTracking().ToListAsync();

            ViewData["ManagerId"] = new SelectList(_context.Managers.Include(c => c.Person).OrderBy(i => i.Person.Surname).AsNoTracking(), "Id", "Name", Order.ManagerId);
            ViewData["OrderStatusId"] = new SelectList(_context.OrderStatuses.OrderBy(i => i.Sequence).AsNoTracking(), "Id", "Name");
            ViewData["AgencyCompanyId"] = new SelectList(_context.AgencyCompanies.OrderBy(i => i.Name).AsNoTracking(), "Id", "Name");
            ViewData["CreateServiceHandler"] = new SelectList(_context.ServiceTypes.OrderBy(st => st.Sequence).AsNoTracking(), "Handler", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool? addCustomerToList, bool? isClose)
        {
            if (!ModelState.IsValid)
                return Page();

            if ((bool)addCustomerToList && Order.CustomerId != null)
            {
                Customer customer = await _context.Customers.Include(c => c.Person).AsNoTracking().FirstOrDefaultAsync(c => c.Id == Order.CustomerId);
                Order.TouristsString = $"{customer.Person.SurnameInitials}\r\n{Order.TouristsString}";
            }

            OrderNumber nextOrderNumber = _context.OrderNumbers.FirstOrDefault();
            Order.Number = nextOrderNumber.Value++;
            _context.Attach(nextOrderNumber).State = EntityState.Modified;

            _context.Attach(Order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(Order.Id))
                    return NotFound();
                else
                    throw;
            }

            if (isClose ?? false)
                return RedirectToPage("./Index");
            else
                return RedirectToPage("./Create", new { Order.Id });
        }

        private bool OrderExists(Guid id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        // Создать Заказчика к Заказу
        public async Task<IActionResult> OnPostCreateCustomerAsync()
        {
            if (!ModelState.IsValid)
                return RedirectToPage("./Create", new { id = Order.Id });

            _context.Attach(Order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            OrderId = Order.Id;
            ReturnPage = Url.Page("Create");

            return RedirectToPage("/AppUsers/Customers/CreateInOrder");
        }

        // Создать Заказчика Физ лицо к Заказу
        public async Task<IActionResult> OnPostCreateCustomerPersonAsync()
        {
            if (!ModelState.IsValid)
                return RedirectToPage("./Create", new { id = Order.Id });

            _context.Attach(Order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            OrderId = Order.Id;
            ReturnPage = Url.Page("Create");

            return RedirectToPage("/AppUsers/Customers/CreateInOrderPerson");
        }

        // Создать Заказчика Юр лицо к Заказу
        public async Task<IActionResult> OnPostCreateCustomerCompanyAsync()
        {
            if (!ModelState.IsValid)
                return RedirectToPage("./Create", new { id = Order.Id });

            _context.Attach(Order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            OrderId = Order.Id;
            ReturnPage = Url.Page("Create");

            return RedirectToPage("/AppUsers/Customers/CreateInOrderCompany");
        }

        // Изменить Заказчика к Заказу
        public async Task<IActionResult> OnPostEditCustomerAsync()
        {
            if (!ModelState.IsValid)
                return RedirectToPage("./Create", new { id = Order.Id });

            _context.Attach(Order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            OrderId = Order.Id;
            ReturnPage = Url.Page("Create");

            return RedirectToPage("/AppUsers/Customers/EditInOrder", new { id = Order.CustomerId });
        }

        // Удалить Заказчика из Заказа
        public async Task<IActionResult> OnPostDeleteCustomerAsync()
        {
            if (!ModelState.IsValid)
                return RedirectToPage("./Create", new { id = Order.Id });

            Order.CustomerId = null;

            _context.Attach(Order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToPage("./Create", "", new { id = Order.Id }, "Customers");
        }

        // Подбор Туроператоров к Заказу
        public async Task<IActionResult> OnPostChoiceTouroperatorsAsync()
        {
            if (!ModelState.IsValid)
                return RedirectToPage("./Create", new { id = Order.Id });

            _context.Attach(Order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            OrderId = Order.Id;
            ReturnPage = Url.Page("Create");

            return RedirectToPage("./ChoiceTouroperators");
        }

        // Удаление Туроператора из Заказа
        public async Task<IActionResult> OnGetDeleteTouroperatorAsync(Guid id, Guid orderId)
        {
            var OrderTouroperatorCompany = await _context.OrderTouroperatorCompanies.FindAsync(id);
            if (OrderTouroperatorCompany != null)
            {
                _context.OrderTouroperatorCompanies.Remove(OrderTouroperatorCompany);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Create", "", new { id = orderId }, "Touroperators");
        }

        // Создание Услуги к Заказу
        public async Task<IActionResult> OnPostCreateServiceAsync(string createServiceHandler)
        {
            if (!ModelState.IsValid)
                return RedirectToPage("./Create", new { id = Order.Id });

            if (string.IsNullOrEmpty(createServiceHandler))
            {
                ErrorMessage = "Не выбран вид услуги!";
                ModelState.AddModelError("Servuces", "Не выбран вид услуги!");
                return RedirectToPage("./Create", "", new { id = Order.Id }, "Services");
            }

            _context.Attach(Order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            OrderId = Order.Id;
            ReturnPage = Url.Page("Create");

            return RedirectToPage($"/Services/{createServiceHandler}/CreateInOrder");
        }

        // Изменение Услуги к Заказу
        public async Task<IActionResult> OnPostEditServiceAsync(Guid editServiceId, string editServiceDiscriminator)
        {
            if (!ModelState.IsValid)
                return RedirectToPage("./Create", new { id = Order.Id });

            _context.Attach(Order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            OrderId = Order.Id;
            ReturnPage = Url.Page("Create");

            return RedirectToPage($"/Services/{editServiceDiscriminator}s/EditInOrder", new { id = editServiceId });
        }

        // Удаление Услуги из Заказа
        public async Task<IActionResult> OnGetDeleteServiceAsync(Guid deleteServiceId, Guid orderId)
        {
            var Service = await _context.Services.FindAsync(deleteServiceId);
            if (Service != null)
            {
                //if (Service.GetType().Name == "PacketTourService")
                if (Service.Discriminator == "PacketTourService")
                {
                    var services = _context.Services.Where(s => s.PacketTourServiceId == deleteServiceId);
                    _context.Services.RemoveRange(services);
                }

                _context.Services.Remove(Service);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Create", "", new { id = orderId }, "Services");
        }

        // Создание Входящего Платежа к Заказу
        public async Task<IActionResult> OnPostCreateIncomingPaymentAsync()
        {
            if (!ModelState.IsValid)
                return RedirectToPage("./Create", new { id = Order.Id });

            _context.Attach(Order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            OrderId = Order.Id;
            ReturnPage = Url.Page("Create");

            return RedirectToPage($"/Payments/IncomingPayments/CreateInOrder");
        }

        // Изменение Входящего платежа к Заказу
        public async Task<IActionResult> OnPostEditIncomingPaymentAsync(Guid editIncomingPaymentId)
        {
            if (!ModelState.IsValid)
                return RedirectToPage("./Create", new { id = Order.Id });

            _context.Attach(Order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            OrderId = Order.Id;
            ReturnPage = Url.Page("Create");

            return RedirectToPage($"/Payments/IncomingPayments/EditInOrder", new { id = editIncomingPaymentId });
        }

        // Удаление Входящего платежа к Заказу
        public async Task<IActionResult> OnGetDeleteIncomingPaymentAsync(Guid deleteIncomingPaymentId, Guid orderId)
        {
            var IncomingPayment = await _context.IncomingPayments.FindAsync(deleteIncomingPaymentId);
            if (IncomingPayment != null)
            {
                _context.IncomingPayments.Remove(IncomingPayment);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Create", "", new { id = orderId }, "Payments");
        }

        // Создание Исходящего Платежа к Заказу
        public async Task<IActionResult> OnPostCreateOutgoingPaymentAsync()
        {
            if (!ModelState.IsValid)
                return RedirectToPage("./Create", new { id = Order.Id });

            _context.Attach(Order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            OrderId = Order.Id;
            ReturnPage = Url.Page("Create");

            return RedirectToPage($"/Payments/OutgoingPayments/CreateInOrder");
        }

        // Изменение Исходящего платежа к Заказу
        public async Task<IActionResult> OnPostEditOutgoingPaymentAsync(Guid editOutgoingPaymentId)
        {
            if (!ModelState.IsValid)
                return RedirectToPage("./Create", new { id = Order.Id });

            _context.Attach(Order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            OrderId = Order.Id;
            ReturnPage = Url.Page("Create");

            return RedirectToPage($"/Payments/OutgoingPayments/EditInOrder", new { id = editOutgoingPaymentId });
        }

        // Удалить Исходящего платежа к Заказу
        public async Task<IActionResult> OnGetDeleteOutgoingPaymentAsync(Guid deleteOutgoingPaymentId, Guid orderId)
        {
            var OutgoingPayment = await _context.OutgoingPayments.FindAsync(deleteOutgoingPaymentId);
            if (OutgoingPayment != null)
            {
                _context.OutgoingPayments.Remove(OutgoingPayment);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Create", "", new { id = orderId }, "Payments");
        }

    }
}
