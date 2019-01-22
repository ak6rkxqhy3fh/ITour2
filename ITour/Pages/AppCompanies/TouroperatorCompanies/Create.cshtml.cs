using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ITour.Data;
using ITour.Models;
using ITour.Services.Tenants;
using Newtonsoft.Json;

namespace ITour.Pages.AppCompanies.TouroperatorCompanies
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ITenantProvider _tenantProvider;

        public CreateModel(ApplicationDbContext context, ITenantProvider tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }

        public IActionResult OnGet()
        {
            ViewData["TouroperatorBrandId"] = new SelectList(_context.TouroperatorBrands, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public TouroperatorCompany TouroperatorCompany { get; set; }

        [BindProperty]
        public TouroperatorBrandCompany TouroperatorBrandCompany { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            TouroperatorCompany.TenantId = _tenantProvider.Tenant.Id;
            TouroperatorCompany.IsOpenData = false;
            TouroperatorCompany.IsValid = true;
            
            FillTouroperatorCompanyJasonData(TouroperatorCompany);

            _context.TouroperatorCompanies.Add(TouroperatorCompany);

            if (TouroperatorBrandCompany.TouroperatorBrandId != Guid.Empty)
            {
                TouroperatorBrandCompany.TenantId = _tenantProvider.Tenant.Id;
                TouroperatorBrandCompany.TouroperatorCompanyId = TouroperatorCompany.Id;

                _context.TouroperatorBrandCompanies.Add(TouroperatorBrandCompany);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private void FillTouroperatorCompanyJasonData(TouroperatorCompany touroperatorCompany)
        {
            Dictionary<string, string> touroperatorDictionary = new Dictionary<string, string>()
            {
                ["Реестровый номер"] = touroperatorCompany.RegistryNumber,
                ["Сокращенное наименование"] = touroperatorCompany.Name,
                ["Полное наименование"] = touroperatorCompany.FullName,
                ["ИНН"] = touroperatorCompany.INN,
                ["ОГРН"] = touroperatorCompany.OGRN,
                ["Юридический адрес"] = touroperatorCompany.AddressLegal,
                ["Адрес (местонахождение)"] = touroperatorCompany.AddressPostal,
                ["Сайт"] = touroperatorCompany.Website,
                ["Общий размер ФО"] = touroperatorCompany.FinGaranteeTotalAmount,
                ["Размер ФО на новый период"] = touroperatorCompany.FinGaranteeAmountNewPeriod,
                ["Действие по на новый период"] = touroperatorCompany.FinGaranteeExpirationDateNewPeriod
            };
            touroperatorCompany.JsonData = JsonConvert.SerializeObject(touroperatorDictionary, Formatting.Indented);
        }
    }
}