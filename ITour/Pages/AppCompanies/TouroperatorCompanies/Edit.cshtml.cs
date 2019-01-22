using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;
using System.Collections.Generic;
using ITour.Services.Tenants;
using Newtonsoft.Json;

namespace ITour.Pages.AppCompanies.TouroperatorCompanies
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ITenantProvider _tenantProvider;

        public EditModel(ApplicationDbContext context, ITenantProvider tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }

        [TempData]
        public string CurrentFilter { get; set; }

        [BindProperty]
        public TouroperatorCompany TouroperatorCompany { get; set; }

        //[BindProperty]
        //public TouroperatorBrandCompany TouroperatorBrandCompany { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id, string currentFilter)
        {
            CurrentFilter = currentFilter;

            if (id == null)
                return NotFound();

            TouroperatorCompany = await _context.TouroperatorCompanies
                .Include(tc => tc.TouroperatorBrands)/*.ThenInclude(tb => tb.TouroperatorBrand)*/
                .FirstOrDefaultAsync(m => m.Id == id);

            if (TouroperatorCompany == null)
                return NotFound();

            if (TouroperatorCompany.IsOpenData) // Нельзя редактировать TouroperatorCompany загруженное из реестра
                return RedirectToPage("./Index", new { currentFilter = CurrentFilter });

            //В результате global query filter по TenantId должнен остаться только один TouroperatorBrandCompany
            if (TouroperatorCompany.TouroperatorBrands.Count == 1) {
                ViewData["TouroperatorBrandCompanyId"] = TouroperatorCompany.TouroperatorBrands[0].Id;
                ViewData["TouroperatorBrandId"] = new SelectList(_context.TouroperatorBrands, "Id", "Name", 
                    TouroperatorCompany.TouroperatorBrands[0].TouroperatorBrandId);
            }
            else if (TouroperatorCompany.TouroperatorBrands.Count > 1)
                throw new Exception("TouroperatorCompany.TouroperatorBrands.Count > 1");
            else
                ViewData["TouroperatorBrandId"] = new SelectList(_context.TouroperatorBrands, "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? touroperatorBrandCompanyId, Guid? touroperatorBrandId)
        {
            if (!ModelState.IsValid)
                return Page();

            FillTouroperatorCompanyJasonData(TouroperatorCompany);

            _context.Attach(TouroperatorCompany).State = EntityState.Modified;

            if (touroperatorBrandCompanyId != null)
            {
                TouroperatorBrandCompany touroperatorBrandCompany = await _context.TouroperatorBrandCompanies.FindAsync(touroperatorBrandCompanyId);

                if (touroperatorBrandId != null)
                {
                    touroperatorBrandCompany.TouroperatorBrandId = (Guid)touroperatorBrandId;
                    _context.TouroperatorBrandCompanies.Update(touroperatorBrandCompany);
                }
                else
                {
                    _context.TouroperatorBrandCompanies.Remove(touroperatorBrandCompany);
                }
            }
            else if (touroperatorBrandId != null)
            {
                TouroperatorBrandCompany touroperatorBrandCompany = new TouroperatorBrandCompany
                {
                    TenantId = _tenantProvider.Tenant.Id,
                    TouroperatorBrandId = (Guid)touroperatorBrandId,
                    TouroperatorCompanyId = TouroperatorCompany.Id
                };
                _context.TouroperatorBrandCompanies.Add(touroperatorBrandCompany);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TouroperatorCompanyExists(TouroperatorCompany.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index", new { currentFilter = CurrentFilter });
        }

        private bool TouroperatorCompanyExists(Guid id)
        {
            return _context.TouroperatorCompanies.Any(e => e.Id == id);
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
