using System;
using System.Threading.Tasks;
using ITour.Data;
using ITour.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

namespace ITour.Pages.Prints
{
    public class PrintModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PrintModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string DocumentContent { get; set; }

        public async Task<IActionResult> OnGet(Guid? orderId, Guid? printId, string contentType)
        {

            PrintTemplate print = await _context.PrintTemplates.FindAsync(printId);

            Order order = await _context.Orders
                .Include(o => o.AgencyCompany).ThenInclude(ac => ac.Person)
                .Include(o => o.Customer).ThenInclude(c => c.Person).ThenInclude(p => p.ApplicationUser)
                .Include(o => o.Customer).ThenInclude(c => c.Person.IdDocument).ThenInclude(d => d.DocumentType)
                .Include(o => o.Customer).ThenInclude(c => c.CustomerCompany)
                .Include(o => o.Manager).ThenInclude(m => m.Person)
                .Include(o => o.Manager).ThenInclude(m => m.PowerAttorneys)
                .Include(o => o.Manager).ThenInclude(m => m.AgencyOffice)
                .Include(o => o.OrderStatus)
                .Include(o => o.TouroperatorBrand)
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
                .Include(o => o.Services).ThenInclude(s => s.CurrencyType)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            DocumentContent = print.GetContent(order);

            if (contentType == "pdf")
            {
                string customSwitches = string.Format(
                    $@"--footer-left ""Подпись: Заказчик____________________"" --footer-right ""Стр: [page] из [toPage]"" --footer-font-size ""8"" --footer-spacing ""2"" ");
                return new ViewAsPdf("ViewAsPdf", DocumentContent)
                {
                    CustomSwitches = customSwitches
                };

            }

            if (contentType == "html")
                return Content(WrapHtml(DocumentContent), "text/html; charset = utf-8");

            return Page();
        }

        string WrapHtml(string content)
        {
            string header =
                    $"<!DOCTYPE html>" +
                    $"<html>" +
                        $"<head>" +
                            $"<meta http-equiv=\"Content-Type\" content=\"text/html; charset = utf-8\" />" +
                            $"<title>Печать</title>" +
                        $"</head >" +
                        $"<body> ";
            string footer =
                    $"</body>" +
                $"</html > ";
            return string.Concat(header, content, footer);
        }
    }
}