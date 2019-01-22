using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ITour.Models
{
    public abstract class AppCompany
    {
        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        [Display(Name = "Удален")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Наименование")]
        public string Name { get; set; }    // Нзвание

        [Display(Name = "Наименование")]
        public string SearchName => Name;   // Название для поиска в форме создания Клиента (что бы не дублировались поля)

        [Display(Name = "Полное наименование")]
        public string FullName { get; set; }    // Полное название

        [Display(Name = "ИНН")]
        public string INN { get; set; }

        [Display(Name = "ОГРН")]
        public string OGRN { get; set; }

        [Display(Name = "Юридический Адрес")]
        public string AddressLegal { get; set; } // Юридический адрес

        [Display(Name = "Почтовый Адрес")]
        public string AddressPostal { get; set; }   // Почтовый адрес

        [Display(Name = "Банковские реквизиты")]
        public string Bank { get; set; }   // Банковские реквизиты

        [Phone]
        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Url]
        [Display(Name = "Вебсайт")]
        public string Website { get; set; }

        [Display(Name = "Примечания")]
        public string Comment { get; set; }
    }

    public class Company : AppCompany
    {

        [Display(Name = "Представитель")]
        public Person Person { get; set; }
        [Display(Name = "Представитель")]
        public Guid? PersonId { get; set; }

        [Display(Name = "Должность представителя РП")]
        public string PersonPost { get; set; }

        [Display(Name = "Действующий на основании РП")]
        public string ActionCause { get; set; }
    }

    public class AgencyCompany : Company
    {
        public AgencyCompany()
        {
            Licenses = new List<License>();
            Orders = new List<Order>();

        }

        public List<License> Licenses { get; set; }

        public List<Order> Orders { get; set; }
    }

    public class AgencyOffice : Company
    {
        public AgencyOffice()
        {
            Managers = new List<Manager>();
            Licenses = new List<License>();
            Orders = new List<Order>();
        }

        public List<Manager> Managers { get; set; }

        public List<License> Licenses { get; set; }

        public List<Order> Orders { get; set; }
    }

    public class License
    {
        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        [Display(Name = "Удален")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Агентство")]
        public AgencyCompany AgencyCompany { get; set; }
        [Display(Name = "Агентство")]
        public Guid? AgencyCompanyId { get; set; }

        [Display(Name = "Офис")]
        public AgencyOffice AgencyOffice { get; set; }
        [Display(Name = "Офис")]
        public Guid? AgencyOfficeId { get; set; }

        [Display(Name = "№")]
        public string LicenseNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата")]
        public DateTime LicenseDate { get; set; }

        [Display(Name = "Лицензия")]
        public string NumberDate => $"№ {LicenseNumber} от {LicenseDate.ToShortDateString()}";

        [Display(Name = "Описание")]
        public string CompanyOffice => $"{AgencyCompany?.Name} - {AgencyOffice?.Name}";
    }


    public class TouroperatorCompany : AppCompany
    {
        public TouroperatorCompany()
        {
            Orders = new List<OrderTouroperatorCompany>();
            TouroperatorBrands = new List<TouroperatorBrandCompany>();
        }
        [Display(Name = "Бренд ТО")]
        public List<TouroperatorBrandCompany> TouroperatorBrands { get; set; }

        [Display(Name = "Реестр №")]
        public string RegistryNumber { get; set; }

        [Display(Name = "Общ разм ФО")]
        public string FinGaranteeTotalAmount { get; set; }     //Общий размер финансового обеспечения

        [Display(Name = "Размер ФО на новый период")]
        public string FinGaranteeAmountNewPeriod { get; set; }     //Размер финансового обеспечения на новый период

        [DataType(DataType.Date)]
        [Display(Name = "Срок ФО на новый период")]
        public string FinGaranteeExpirationDateNewPeriod { get; set; }         // Дата действия финансового обеспечения на новый период

        public DateTime? FinGaranteeExpirationDateNewPeriodDate
        {
            get
            {
                if (!string.IsNullOrEmpty(FinGaranteeExpirationDateNewPeriod) && !string.Equals(FinGaranteeExpirationDateNewPeriod, "0000-00-00"))
                    return Convert.ToDateTime(FinGaranteeExpirationDateNewPeriod);
                return null;
            }
        }

        public string FinGaranteeExpirationDateNewPeriodString =>
            FinGaranteeExpirationDateNewPeriodDate != null ? ((DateTime)FinGaranteeExpirationDateNewPeriodDate).ToShortDateString() : "";

        public bool IsFinGaranteeDateNewPeriodExpired => FinGaranteeExpirationDateNewPeriodDate < DateTime.Now || FinGaranteeExpirationDateNewPeriodDate == null;

        public string JsonData { get; set; }                    // Остальная информация о туроператоре

        public Dictionary<string, string> DictionaryData =>
            !string.IsNullOrEmpty(JsonData) ? JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonData) : null;

        public bool IsValid { get; set; }          // Признак того, что ТО исключен из реестра

        public bool IsOpenData { get; set; }        // Признак того, что ТО загружен с Ростуризма (Open Data)

        public List<OrderTouroperatorCompany> Orders { get; set; }

        public string ShortInfo
        {
            get

           {
                string shortInfo = "";
                int countInfo = 6;
                if (DictionaryData != null)
                {
                    foreach (KeyValuePair<string, string> data in DictionaryData)
                    {
                        if (countInfo < 0) break;
                        shortInfo += $"{data.Key}: {data.Value}, ";
                        countInfo--;
                    }
                }
                return shortInfo;
            }
        }

        public string FullInfo
        {
            get
            {
                string fullInfo = "";
                if (DictionaryData != null)
                {
                    foreach (KeyValuePair<string, string> data in DictionaryData)
                    {
                        fullInfo += $"{data.Key}: {data.Value}, ";
                    }
                }
                return fullInfo;
            }

        }
    }

    public class TouroperatorBrand
    {
        public TouroperatorBrand()
        {
            TouroperatorCompanies = new List<TouroperatorBrandCompany>();
            Orders = new List<Order>();
        }
        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        [Display(Name = "Удален")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Туроператор")]
        public string Name { get; set; }

        [Display(Name = "Юр. лица ТО")]
        public List<TouroperatorBrandCompany> TouroperatorCompanies { get; set; }

        public List<Order> Orders { get; set; }
    }

    public class TouroperatorBrandCompany
    {
        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        [Display(Name = "Юр. лицо ТО")]
        public TouroperatorCompany TouroperatorCompany { get; set; }
        public Guid TouroperatorCompanyId { get; set; }

        [Display(Name = "Туроператор")]
        public TouroperatorBrand TouroperatorBrand { get; set; }
        public Guid TouroperatorBrandId { get; set; }
    }

    public class CustomerCompany : Company { }

    public class InsuranceCompany : Company { }

    public class PartnerCompany : Company
    {
        public PartnerCompany()
        {
            OutgoingPayments = new List<OutgoingPayment>();
        }
        public List<OutgoingPayment> OutgoingPayments { get; set; }
    }
}
