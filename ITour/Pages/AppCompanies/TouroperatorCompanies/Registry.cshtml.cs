using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITour.Data;
using ITour.Models;
using ITour.Models.RegistryTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ITour.Pages.AppCompanies.TouroperatorCompanies
{
    public class RegistryModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RegistryModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public RegistryUri RegistryUri { get; set; }

        public RegistryStatistic RegistryStat { get; set; }

        public class RegistryStatistic
        {
            public int Count { get; set; }
            public int CountValid { get; set; }
            public int CountOpenData { get; set; }
        }

        public void OnGet()
        {
            List<TouroperatorCompany> TouroperatorCompanies = _context.TouroperatorCompanies.AsNoTracking().ToList();
            if(TouroperatorCompanies.Count > 0)
            {
                RegistryStat = new RegistryStatistic
                {
                    Count = TouroperatorCompanies.Count(),
                    CountValid = TouroperatorCompanies.Where(t => t.IsValid).Count(),
                    CountOpenData = TouroperatorCompanies.Where(t => t.IsValid).Where(t => t.IsOpenData).Count()
                };
            }
            RegistryUri = _context.RegistryUris.FirstOrDefault();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _context.Attach(RegistryUri).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostLoadAsync()
        {
            Touroperators.Registry touroperatorsRegistry = new Touroperators.Registry();

            List<Touroperators.OpenData> openDataList = await touroperatorsRegistry.LoadOpenDataAsync(openDataUri: RegistryUri.UriString);

            foreach (Touroperators.OpenData openData in openDataList)
            {
                string regionString = await openData.GetRegionStringAsync();

                if (!string.IsNullOrEmpty(regionString))
                {
                    List<Dictionary<string, string>> regionData = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(regionString);

                    foreach (Dictionary<string, string> touroperatorDictionary in regionData)
                    {
                            TouroperatorCompany touroperatorCompany = new TouroperatorCompany
                            {
                                RegistryNumber = touroperatorDictionary["Реестровый номер"] ?? "",
                                Name = touroperatorDictionary["Сокращенное наименование"] ?? "",
                                Website = $"http://{touroperatorDictionary["Сайт"] ?? ""}",
                                FinGaranteeTotalAmount = touroperatorDictionary["Общий размер ФО"] ?? "",
                                JsonData = JsonConvert.SerializeObject(touroperatorDictionary, Formatting.Indented),
                                IsValid = true,
                                IsOpenData = true,
                            };
                            if (touroperatorDictionary.ContainsKey("Размер ФО на новый период"))
                                touroperatorCompany.FinGaranteeAmountNewPeriod = touroperatorDictionary["Размер ФО на новый период"] ?? "";
                            if (touroperatorDictionary.ContainsKey("Действие по на новый период"))
                                touroperatorCompany.FinGaranteeExpirationDateNewPeriod = touroperatorDictionary["Действие по на новый период"] ?? "";

                            _context.Add(touroperatorCompany);
                    }
                    _context.SaveChanges();
                }
            }
            //return Page();
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            // Вначале флаг валидности установить в false - потом по мере обновления вернуть в true            
            int numberOfRowUpdated =
                _context.Database.ExecuteSqlCommand("UPDATE [dbo].[TouroperatorCompanies] SET [IsValid] = 0 WHERE [IsOpenData] = 1");

            Touroperators.Registry touroperatorsRegistry = new Touroperators.Registry();

            List<Touroperators.OpenData> openDataList = await touroperatorsRegistry.LoadOpenDataAsync(openDataUri: RegistryUri.UriString);

            foreach (Touroperators.OpenData openData in openDataList)
            {
                string regionString = await openData.GetRegionStringAsync();

                if (!string.IsNullOrEmpty(regionString))
                {
                    List<Dictionary<string, string>> regionData = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(regionString);

                    foreach (Dictionary<string, string> touroperatorDictionary in regionData)
                    {
                        bool touroperatorNew = false;

                        TouroperatorCompany touroperatorCompany = _context.TouroperatorCompanies
                            .Where(to => to.RegistryNumber == touroperatorDictionary["Реестровый номер"]).FirstOrDefault();

                        if (touroperatorCompany == null)
                        {
                            touroperatorCompany = new TouroperatorCompany
                            {
                                RegistryNumber = touroperatorDictionary["Реестровый номер"]
                            };

                            touroperatorNew = true;
                        }

                        touroperatorCompany.Name = touroperatorDictionary["Сокращенное наименование"] ?? "";
                        touroperatorCompany.FinGaranteeTotalAmount = touroperatorDictionary["Общий размер ФО"] ?? "";
                        touroperatorCompany.Website = $"http://{touroperatorDictionary["Сайт"] ?? ""}";
                        touroperatorCompany.JsonData = JsonConvert.SerializeObject(touroperatorDictionary, Formatting.Indented);
                        touroperatorCompany.IsValid = true;
                        touroperatorCompany.IsOpenData = true;

                        if (touroperatorDictionary.ContainsKey("Размер ФО на новый период"))
                            touroperatorCompany.FinGaranteeAmountNewPeriod = touroperatorDictionary["Размер ФО на новый период"] ?? "";
                        if (touroperatorDictionary.ContainsKey("Действие по на новый период"))
                            touroperatorCompany.FinGaranteeExpirationDateNewPeriod = touroperatorDictionary["Действие по на новый период"] ?? "";

                        if (touroperatorNew)
                        {
                            _context.TouroperatorCompanies.Add(touroperatorCompany);
                        }
                        else
                        {
                            _context.TouroperatorCompanies.Update(touroperatorCompany);
                        }
                    }
                    _context.SaveChanges();
                }
            }
            //return Page();
            return RedirectToPage("./Index");
        }

        public IActionResult OnPostClear()
        {
            List<TouroperatorCompany> TouroperatorCompanies = _context.TouroperatorCompanies.ToList();
            _context.RemoveRange(TouroperatorCompanies);
            _context.SaveChanges();
            return Page();
            //return RedirectToPage("./Index");
        }
    }
}