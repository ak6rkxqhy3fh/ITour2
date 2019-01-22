using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ITour.Models
{
    // Базовый пользователь
    public abstract class AppUser/* : AppEntity*/
    {
        public AppUser()
        {
            Orders = new List<Order>();
        }
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }

        [Display(Name = "Удален")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Физ. лицо")]
        public Person Person { get; set; }
        [Display(Name = "Физ. лицо")]
        public Guid? PersonId { get; set; }

        public List<Order> Orders { get; set; }
    }

    public class ApplicationUser : IdentityUser
    {
        public Guid? TenantId { get; set; }
        [Display(Name = "Удален")]
        public bool IsDeleted { get; set; }
    }

    public class Person
    {
        public Person()
        {
            PersonFiles = new List<PersonFile>();
        }

        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        [Display(Name = "Удален")]
        public bool IsDeleted { get; set; }

        // email и телефон хранятся в ApplicationUser

        [Display(Name = "User")]
        public ApplicationUser ApplicationUser { get; set; }
        [Display(Name = "User")]
        public string ApplicationUserId { get; set; }

        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Display(Name = "Имя")]
        public string Firstname { get; set; }

        [Display(Name = "Отчество")]
        public string Middlename { get; set; }

        [Display(Name = "Фамилия, инициалы")]
        public string SurnameInitials => $"{Surname} {Firstname?.Substring(0, 1)}. {Middlename?.Substring(0, 1)}.";

        [Display(Name = "ФИО полностью")]
        public string FullName => $"{Surname} {Firstname} {Middlename}";

        [DataType(dataType: DataType.Date)]
        [Display(Name = "Дата рождения")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Месяц рождения рождения")]
        public int BirthDateMonth => BirthDate != null ? ((DateTime)BirthDate).Month : 0;

        public bool IsBirthDayToday =>
            (BirthDate != null && ((DateTime)BirthDate).Month == DateTime.Today.Month && ((DateTime)BirthDate).Day == DateTime.Today.Day) ? true : false;

        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Display(Name = "Примечания")]
        public string Comment { get; set; }


        [Display(Name = "Фамилия в родительном падеже")]
        public string SurnameGenitive { get; set; }

        [Display(Name = "Имя в родительном падеже")]
        public string FirstnameGenitive { get; set; }

        [Display(Name = "Отчество в родительном падеже")]
        public string MiddlenameGenitive { get; set; }

        [Display(Name = "Фамилия, инициалы в родит падеже")]
        public string SurnameInitialsGenitive => $"{SurnameGenitive} {FirstnameGenitive?.Substring(0, 1)}. {MiddlenameGenitive?.Substring(0, 1)}.";

        [Display(Name = "ФИО в родительном падеже")]
        public string FullNameGenitive => $"{SurnameGenitive} {FirstnameGenitive} {MiddlenameGenitive}";

        [Display(Name = "Осн. документ")]
        public Document IdDocument { get; set; }

        [Display(Name = "Осн. документ")]
        public string IdDocumentInfo => $"{IdDocument?.DocumentType?.Name} {IdDocument?.Series} {IdDocument?.Number} {IdDocument?.IssuedBy} {IdDocument?.IssuedDateString}";

        [Display(Name = "Загранпаспорт")]
        public Document Passport { get; set; }

        [Display(Name = "Файлы")]
        public List<PersonFile> PersonFiles { get; set; }

        [Display(Name = "Сотрудник")]
        public bool IsEmployee { get; set; }
    }

    [Owned]
    public class Document
    {
        [Display(Name = "Вид документа")]
        public DocumentType DocumentType { get; set; }
        [Display(Name = "Вид документа")]
        public Guid? DocumentTypeId { get; set; }

        [Display(Name = "Серия")]
        public string Series { get; set; }

        [Display(Name = "Номер")]
        public string Number { get; set; }

        [Display(Name = "Кем выдан")]
        public string IssuedBy { get; set; }

        [DataType(dataType: DataType.Date)]
        [Display(Name = "Когда выдан")]
        public DateTime? IssuedDate { get; set; }

        [Display(Name = "Когда выдан")]
        public string IssuedDateString => IssuedDate != null ? ((DateTime)IssuedDate).ToShortDateString() : "";

        [DataType(dataType: DataType.Date)]
        [Display(Name = "Срок действия")]
        public DateTime? ExpirationDate { get; set; }

        [Display(Name = "Срок действия")]
        public string ExpirationDateString => ExpirationDate != null ? ((DateTime)ExpirationDate).ToShortDateString() : "";

    }

    public class DocumentType : AppType { }

    public class PersonFile : AppFile
    {
        [Display(Name = "Персона")]
        public Person Person { get; set; }
        [Display(Name = "Персона")]
        public Guid? PersonId { get; set; }
    }

    // Менеджер
    public class Manager : AppUser
    {
        public Manager() : base()
        {
            PowerAttorneys = new List<PowerAttorney>();
            Customers = new List<Customer>();
        }

        [Display(Name = "Менеджер")]
        public string Name => $"{Person?.SurnameInitials}";

        [Display(Name = "Офис")]
        public AgencyOffice AgencyOffice { get; set; }
        [Display(Name = "Офис")]
        public Guid? AgencyOfficeId { get; set; }

        public List<PowerAttorney> PowerAttorneys { get; set; }    // Доверенности  
        public List<Customer> Customers { get; set; }
    }

    // Доверенность
    public class PowerAttorney
    {
        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        [Display(Name = "Удален")]
        public bool IsDeleted { get; set; }

        [Display(Name = "№")]
        public string Number { get; set; }

        [DataType(dataType: DataType.Date)]
        [Display(Name = "Когда выдана")]
        public DateTime? IssuedDate { get; set; }

        [Display(Name = "Менеджер")]
        public Manager Manager { get; set; }

        [Display(Name = "Менеджер")]
        public Guid ManagerId { get; set; }

        public string Print => (Number != null && IssuedDate != null) ? $"№ {Number} от {((DateTime)IssuedDate).ToShortDateString()}" : "";
    }

    // Заказчик
    public class Customer : AppUser
    {
        public Customer() : base()
        {
            CustomerFiles = new List<CustomerFile>();
        }

        [Display(Name = "Заказчик")]
        public string Name
        {
            get
            {
                string name = "";
                if (CustomerCompany != null)
                    name = $"{ CustomerCompany?.Name} ({Person?.SurnameInitials})";
                else
                    name = $"{Person?.SurnameInitials}";
                return name;
            }
        }

        [Display(Name = "Заказчик")]
        public string FullPersonName
        {
            get
            {
                string name = "";
                if (CustomerCompany != null)
                    name = $"{CustomerCompany?.Name} ({Person?.FullName})";
                else
                    name = $"{Person?.FullName}";   
                return name;
            }
        }

        [Display(Name = "Менеджер")]
        public Manager Manager { get; set; }
        [Display(Name = "Менеджер")]
        public Guid? ManagerId { get; set; }

        [Display(Name = "Юр. Лицо")]
        public CustomerCompany CustomerCompany { get; set; }
        [Display(Name = "Юр. Лицо")]
        public Guid? CustomerCompanyId { get; set; }

        [Display(Name = "Файлы")]
        public List<CustomerFile> CustomerFiles { get; set; }
    }

    public class CustomerFile : AppFile
    {
        [Display(Name = "Заказчик")]
        public Customer Customer { get; set; }
        [Display(Name = "Заказчик")]
        public Guid? CustomerId { get; set; }
    }
}
