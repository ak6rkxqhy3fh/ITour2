using RSDN;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ITour.Models
{
    public class Service
    {
        public Service()
        {
            Cost = 0;
        }
        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        [Display(Name = "Удален")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Заказ")]
        public Order Order { get; set; }
        [Display(Name = "Заказ")]
        public Guid OrderId { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Примечания")]
        public string Comment { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата начала")]
        public DateTime? DateBegin { get; set; }
        public string DateBeginString => $"{((DateBegin != null) ? ((DateTime)DateBegin).ToShortDateString() : "")}";

        [DataType(DataType.Time)]
        [Display(Name = "Время начала")]
        public TimeSpan? TimeBegin { get; set; }
        public string TimeBeginString => $"{((TimeBegin != null) ? ((TimeSpan)TimeBegin).ToString(@"hh\:mm") : "")}";

        [DataType(DataType.Date)]
        [Display(Name = "Дата окончания")]
        public DateTime? DateEnd { get; set; }
        public string DateEndString => $"{((DateEnd != null) ? ((DateTime)DateEnd).ToShortDateString() : "")}";

        [DataType(DataType.Time)]
        [Display(Name = "Время окончания")]
        public TimeSpan? TimeEnd { get; set; }
        public string TimeEndString => $"{((TimeEnd != null) ? ((TimeSpan)TimeEnd).ToString(@"hh\:mm") : "")}";

        [Column(TypeName = "decimal(9, 2)"), DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Стоимость")]
        public decimal? Cost { get; set; }

        [Display(Name = "Стоимость")]
        public string CostCurrency => Cost != null ? (Cost != 0 ? $"{((decimal)Cost).ToString("C")}" : " - ") : "";
        [Display(Name = "Стоимость")]
        public string CostNumeric => Cost != null ? (Cost != 0 ? $"{((decimal)Cost).ToString("N")}" : " 0 ") : "";

        [Display(Name = "Стоимость прописью")]
        public string CostString => Cost != null ? $"{RusCurrency.Str((double)Cost)}" : "";

        [Column(TypeName = "decimal(9, 2)"), DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Себестоимость")]
        public decimal? CostPrice { get; set; }

        [Display(Name = "Себестоимость")]
        public string CostPriceCurrency => CostPrice != null ? (CostPrice != 0 ? $"{((decimal)CostPrice).ToString("C")}" : " - ") : "";
        [Display(Name = "Себестоимость")]
        public string CostPriceNumeric => CostPrice != null ? (CostPrice != 0 ? $"{((decimal)CostPrice).ToString("N")}" : " 0 ") : "";

        [Display(Name = "Валюта")]
        public CurrencyType CurrencyType { get; set; }
        [Display(Name = "Валюта")]
        public Guid? CurrencyTypeId { get; set; }

        [Column(TypeName = "decimal(9, 2)"), DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Курс")]
        public decimal? ExchangeRate { get; set; }

        public PacketTourService PacketTourService { get; set; }
        public Guid? PacketTourServiceId { get; set; }

        public string Discriminator { get; set; }

        public int Sequence { get; set; }

        public virtual string Details
        {
            get
            {
                string details = $"{CostCurrency} | {Name} | {Description} | {Comment} | {DateBeginString} - {TimeBeginString} | {DateEndString} - {TimeEndString}";
                return details;
            }
        }

        public virtual string DetailsPrint
        {
            get
            {
                string details = "";
                return details;
            }
        }
    }

    public class ServiceFilter<T> where T : Service
    {
        [Display(Name = "Заказ")]
        public int? OrderNumber { get; set; }

        [Display(Name = "Период с")]
        [DataType(DataType.Date)]
        public DateTime? PeriodFrom { get; set; }
        [Display(Name = "Период по")]
        [DataType(DataType.Date)]
        public DateTime? PeriodTo { get; set; }

        [Display(Name = "Менеджер")]
        public Guid? ManagerId { get; set; }
        [Display(Name = "Заказчик")]
        public string CustomerName { get; set; }

        public bool NotAllParamsIsNull => OrderNumber != null || PeriodFrom != null || PeriodTo != null || ManagerId != null || CustomerName != null;

        public IQueryable<T> Process(IQueryable<T> serviceIQ)
        {
            if (OrderNumber != null)
                serviceIQ = serviceIQ.Where(s => s.Order.Number == OrderNumber);

            if (PeriodFrom != null)
                serviceIQ = serviceIQ.Where(s => s.DateBegin >= PeriodFrom);

            if (PeriodTo != null)
                serviceIQ = serviceIQ.Where(s => s.DateBegin <= PeriodTo);

            if (ManagerId != null)
                serviceIQ = serviceIQ.Where(s => s.Order.ManagerId == ManagerId);

            if (CustomerName != null)
                serviceIQ = serviceIQ.Where(s =>
                s.Order.Customer.Person.Surname.Contains(CustomerName) || s.Order.Customer.CustomerCompany.Name.Contains(CustomerName));

            return serviceIQ;
        }
    }

    public class ServiceType : ApplicationType
    {
        public string Handler { get; set; }
    }

    public class CurrencyType : AppType { }

    public static class ServiceStyle
    {
        public static string table = "style = \" border: double black thin; border-collapse: collapse; width: 100%; \"";
        public static string td = "style = \"border: solid grey thin; text-align: center; padding: 5px;\"";
    }

    public class PacketTourService : Service
    {
        public PacketTourService() : base()
        {
            Services = new List<Service>();
            Name = "Пакетный тур";
            Sequence = 10;
        }

        public List<Service> Services { get; set; }

        public override string Details
        {
            get
            {
                string details = $"{CostCurrency} | Пакетный тур - {Name} | {Description} | {Comment} | {DateBeginString} - {TimeBeginString} | {DateEndString} - {TimeEndString}";
                return details;
            }
        }

        public override string DetailsPrint
        {
            get
            {
                string details = base.DetailsPrint;
                details += $"<table {ServiceStyle.table} >" +
                                $"<tr>" +
                                    $"<td {ServiceStyle.td} >Пакетный тур</td>" +
                                    (!string.IsNullOrEmpty(Description) ? $"<td {ServiceStyle.td} >Описание</td>" : "") +
                                    (!string.IsNullOrEmpty(Comment) ? $"<td {ServiceStyle.td} >Примечания</td>" : "") +
                                    $"<td {ServiceStyle.td} ><div>Стоимость</td>" +
                                $"</tr>" +
                                $"<tr>" +
                                    (!string.IsNullOrEmpty(Name) ? $"<td {ServiceStyle.td} >{Name}</td>" : $"<td {ServiceStyle.td} >Стандартный пакетный тур</td>") +
                                    (!string.IsNullOrEmpty(Description) ? $"<td {ServiceStyle.td} >{Description}</td>" : "") +
                                    (!string.IsNullOrEmpty(Comment) ? $"<td {ServiceStyle.td} >{Comment}</td>" : "") +
                                    $"<td {ServiceStyle.td} ><div>{CostCurrency}</div><div>(по курсу 1 {CurrencyType?.Name} = {ExchangeRate}&#8381;)</div></td>" +
                                $"</tr>" +
                            $"</table>";
                return details;
            }
        }
    }

    public class AccomodationService : Service
    {
        public AccomodationService() : base()
        {
            Name = "Размещение";
            Sequence = 20;
        }

        [Display(Name = "Страна")]
        public Country Country { get; set; }
        [Display(Name = "Страна")]
        public Guid? CountryId { get; set; }

        [Display(Name = "Курорт")]
        public Resort Resort { get; set; }
        [Display(Name = "Курорт")]
        public Guid? ResortId { get; set; }

        [Display(Name = "Отель")]
        public Hotel Hotel { get; set; }
        [Display(Name = "Отель")]
        public Guid? HotelId { get; set; }

        [Display(Name = "Тип питания")]
        public FoodType FoodType { get; set; }
        [Display(Name = "Питание")]
        public Guid? FoodTypeId { get; set; }
        [Display(Name = "Питание (нетиповое)")]
        public string FoodTypeString { get; set; }

        [Display(Name = "Номер")]
        public RoomType RoomType { get; set; }
        [Display(Name = "Номер")]
        public Guid? RoomTypeId { get; set; }
        [Display(Name = "Номер (нетиповой)")]
        public string RoomTypeString { get; set; }

        public override string Details
        {
            get
            {
                string details = base.Details;
                details += $" | {Country?.Name} - {Resort?.Name} - {Hotel?.Name} ({Hotel?.NameEn}) | {FoodType?.Name} - {FoodTypeString} | {RoomType?.Name} - {RoomTypeString} | ";
                return details;
            }
        }

        public override string DetailsPrint
        {
            get
            {
                string details = base.DetailsPrint;
                details += $"<table {ServiceStyle.table} >" +
                                $"<tr>" +
                                    $"<td {ServiceStyle.td} >Страна</td>" +
                                    $"<td {ServiceStyle.td} >Курорт</td>" +
                                    $"<td {ServiceStyle.td} >Отель</td>" +
                                    $"<td {ServiceStyle.td} >Даты</td>" +
                                    $"<td {ServiceStyle.td} >Номер</td>" +
                                    $"<td {ServiceStyle.td} >Питание</td>" +
                                    (!string.IsNullOrEmpty(Comment) ? $"<td {ServiceStyle.td} >Примечания</td>" : "") +
                                    (Cost != 0 ? $"<td {ServiceStyle.td} >Стоимость</td>" : "") +
                                $"</tr>" +
                                $"<tr>" +
                                    $"<td {ServiceStyle.td} >{Country?.Name}</td>" +
                                    $"<td {ServiceStyle.td} >{Resort?.Name}</td>" +
                                    $"<td {ServiceStyle.td} >" +
                                        $"<div>{Hotel?.Name}</div>" +
                                        (!string.IsNullOrEmpty(Hotel?.NameEn) ? $"<div>({Hotel?.NameEn})</div>" : "") +
                                    $"</td>" +
                                    $"<td {ServiceStyle.td} >{DateBeginString} - {DateEndString}</td>" +
                                    $"<td {ServiceStyle.td} >{RoomType?.Name} {RoomTypeString}</td>" +
                                    $"<td {ServiceStyle.td} >{FoodType?.Name} {FoodTypeString}</td>" +
                                    (!string.IsNullOrEmpty(Comment) ? $"<td {ServiceStyle.td} >{Comment}</td>" : "") +
                                    (Cost != 0 ? $"<td {ServiceStyle.td} >{CostCurrency}</td>" : "") +
                                $"</tr>" +
                            $"</table>";
                return details;
            }
        }

        public string ShortDetails
        {
            get
            {
                string details = "";
                details += $"<div>{Country?.Name} {Resort?.Name}</div><div>{Hotel?.NameEn}</div>";
                return details;
            }
        }
    }

    public class Country
    {
        public Country()
        {
            Resorts = new List<Resort>();
        }

        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        [Display(Name = "Удален")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Страна")]
        public string Name { get; set; }

        public List<Resort> Resorts { get; set; }
    }

    public class Resort
    {
        public Resort()
        {
            Hotels = new List<Hotel>();
        }

        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        [Display(Name = "Удален")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Страна")]
        public Country Country { get; set; }
        [Display(Name = "Страна")]
        public Guid? CountryId { get; set; }
        [Display(Name = "Курорт")]
        public string Name { get; set; }


        [Display(Name = "Отели")]
        public List<Hotel> Hotels { get; set; }
    }

    public class Hotel
    {
        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        [Display(Name = "Удален")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Курорт")]
        public Resort Resort { get; set; }
        [Display(Name = "Курорт")]
        public Guid? ResortId { get; set; }

        [Display(Name = "Отель")]
        public string Name { get; set; }

        [Display(Name = "Отель (англ)")]
        public string NameEn { get; set; }

        [Display(Name = "Отель")]
        public string NameSelect => $"{Name} ({NameEn})";

    }

    public class RoomType : AppType { }

    public class FoodType : AppType { }

    public class TransportService : Service
    {
        public TransportService() : base()
        {
            Name = "Транспорт";
            Sequence = 30;
        }

        [Display(Name = "Вид")]
        public TransportType TransportType { get; set; } //  (авиа, ж/д, паром и т.д.)
        [Display(Name = "Вид")]
        public Guid? TransportTypeId { get; set; }

        [Display(Name = "Рейс")]
        public string TripNumber { get; set; }

        [Display(Name = "Город отправления")]
        public string DepartureSity { get; set; }
        [Display(Name = "Терминал")]
        public string DepartureTerminal { get; set; }

        [Display(Name = "Город прибытия")]
        public string ArrivalSity { get; set; }
        [Display(Name = "Терминал")]
        public string ArrivalTerminal { get; set; }

        [Display(Name = "Проездные документы")]
        public string Tickets { get; set; }

        [Display(Name = "Рейс")]
        public string TripPrint  => $"<table>" +
                $"<tr><td>{TransportType?.Name ?? ""}</td></tr>" +
                $"<tr><td>{TripNumber ?? ""}</td></tr>" +
            $"</table>";

        [Display(Name = "Отправление")]
        public string DeparturePrint =>
            $"<table>" +
                $"<tr><td>{DepartureSity} - {DepartureTerminal}</td></tr>" +
                $"<tr><td>{DateBeginString} - {TimeBeginString}</td></tr>" +
            $"</table>";

        [Display(Name = "Прибытие")]
        public string ArrivalPrint =>
            $"<table>" +
                $"<tr><td>{ArrivalSity} - {ArrivalTerminal}</td></tr>" +
                $"<tr><td>{DateEndString} - {TimeEndString}</td></tr>" +
            $"</table>";

        public override string Details
        {
            get
            {
                string details = base.Details;
                details += $" | {TransportType?.Name} | {TripNumber} | {DepartureSity} - {DepartureTerminal} | {ArrivalSity} - {ArrivalTerminal} | {Tickets} |";
                return details;
            }
        }

        public override string DetailsPrint
        {
            get
            {
                string details = base.DetailsPrint;
                details += $"<table {ServiceStyle.table}>" +
                                $"<tr>" +
                                    $"<td {ServiceStyle.td}>Транспорт</td>" +
                                    $"<td {ServiceStyle.td}>Рейс</td>" +
                                    $"<td {ServiceStyle.td}>Отправление</td>" +
                                    $"<td {ServiceStyle.td}>Прибытие</td>" +
                                    $"<td {ServiceStyle.td}>Проездн док</td>" +
                                    (!string.IsNullOrEmpty(Comment) ? $"<td {ServiceStyle.td}>Примечания</td>" : "") +
                                    (Cost != 0 ? $"<td {ServiceStyle.td}>Стоимость</td>" : "") +
                                $"</tr>" +
                                $"<tr>" +
                                    $"<td {ServiceStyle.td}>{TransportType?.Name}</td>" +
                                    $"<td {ServiceStyle.td}>{TripNumber}</td>" +
                                    $"<td {ServiceStyle.td}><div>{DepartureSity} {DepartureTerminal}</div><div>{DateBeginString} {TimeBeginString}</div></td>" +
                                    $"<td {ServiceStyle.td}><div>{ArrivalSity} {ArrivalTerminal}</div><div>{DateEndString} {TimeEndString}</div></td>" +
                                    $"<td {ServiceStyle.td}>{Tickets}</td>" +
                                    (!string.IsNullOrEmpty(Comment) ? $"<td {ServiceStyle.td}>{Comment}</td>" : "") +
                                    (Cost != 0 ? $"<td {ServiceStyle.td}>{CostCurrency}</td>" : "") +
                                $"</tr>" +
                            $"</table>";
                return details;
            }
        }
    }

    public class TransportType : AppType { }

    public class FuelSurchargeService : Service
    {
        public FuelSurchargeService() : base()
        {
            Name = "Топливный сбор";
            Sequence = 40;
        }

        [Display(Name = "Топливный сбор")]
        public bool IsFuelSurcharge { get; set; }

        public override string Details
        {
            get
            {
                string details = base.Details;
                details += $" | {(IsFuelSurcharge ? "Есть" : "Нет")} | ";
                return details;
            }
        }

        public override string DetailsPrint
        {
            get
            {
                string details = base.DetailsPrint;
                details += $"<table {ServiceStyle.table}>" +
                                $"<tr>" +
                                    $"<td {ServiceStyle.td}>Топливный сбор</td>" +
                                    $"<td {ServiceStyle.td}>{(IsFuelSurcharge ? "Есть" : "Нет")}</td>" +
                                    (!string.IsNullOrEmpty(Comment) ? $"<td {ServiceStyle.td}>{Comment}</td>" : "") +
                                    (Cost != 0 ? ($"<td {ServiceStyle.td}>Стоимость: {CostCurrency} </td>") : "") +
                                $"</tr>" +
                            $"</table>";
                return details;
            }
        }
    }

    public class MaintenanceService : Service
    {
        public MaintenanceService()
        {
            Name = "Встречи, проводы, сопровождение";
            Description = "Регистрация на рейс в а/порту за 2,5 часа до вылета. Получение документов в Турагентстве за 2-3 дня до вылета.";
            Sequence = 50;
        }

        public override string Details
        {
            get
            {
                string details = base.Details;
                return details;
            }
        }

        public override string DetailsPrint
        {
            get
            {
                string details = base.DetailsPrint;
                details += $"<table {ServiceStyle.table}>" +
                                $"<tr>" +
                                    $"<td {ServiceStyle.td}>{Name}</td>" +
                                    (!string.IsNullOrEmpty(Description) ? $"<td {ServiceStyle.td}>{Description}</td>" : "Нет") +
                                    (!string.IsNullOrEmpty(Comment) ? $"<td {ServiceStyle.td}>{Comment}</td>" : "") +
                                    (Cost != 0 ? ($"<td {ServiceStyle.td}>Стоимость: {CostCurrency} </td>") : "") +
                                $"</tr>" +
                            $"</table>";
                return details;
            }
        }
    }

    public class TransferService : Service
    {
        public TransferService() : base()
        {
            Name = "Трансфер";
            Sequence = 60;
        }

        [Display(Name = "Вид")]
        public TransferType TransferType { get; set; }

        [Display(Name = "Вид")]
        public Guid? TransferTypeId { get; set; }

        public override string Details
        {
            get
            {
                string details = base.Details;
                details += $" | {(TransferType?.Name ?? "Нет")} | ";
                return details;
            }
        }

        public override string DetailsPrint
        {
            get
            {
                string details = base.DetailsPrint;
                details += $"<table {ServiceStyle.table}>" +
                                $"<tr>" +
                                    $"<td {ServiceStyle.td}>Трансфер</td>" +
                                    $"<td {ServiceStyle.td}>{TransferType?.Name ?? "Нет"}</td>" +
                                    (!string.IsNullOrEmpty(Comment) ? $"<td {ServiceStyle.td}>{Comment}</td>" : "") +
                                    (Cost != 0 ? ($"<td {ServiceStyle.td}>Стоимость: {CostCurrency} </td>") : "") +
                                $"</tr>" +
                            $"</table>";
                return details;
            }
        }
    }

    public class TransferType : AppType { }

    public class InsuranceService : Service
    {
        public InsuranceService() : base()
        {
            Name = "Страховка";
            Sequence = 70;
        }

        [Display(Name = "Вид")]
        public InsuranceType InsuranceType { get; set; }
        [Display(Name = "Вид")]
        public Guid? InsuranceTypeId { get; set; }

        [Display(Name = "Страховщик")]
        public InsuranceCompany InsuranceCompany { get; set; }
        [Display(Name = "Страховщик")]
        public Guid? InsuranceCompanyId { get; set; }

        public override string Details
        {
            get
            {
                string details = base.Details;
                details += $" | {InsuranceType?.Name} | {InsuranceCompany?.Name} | ";
                return details;
            }
        }

        public override string DetailsPrint
        {
            get
            {
                string details = base.DetailsPrint;
                string insuranceCompanyInfo = InsuranceCompany == null ? "Нет" :
                    $"{InsuranceCompany.FullName} {InsuranceCompany.AddressLegal} {InsuranceCompany.Phone} {InsuranceCompany.Email} {InsuranceCompany.Website} {InsuranceCompany.Comment}";
                details += $"<table {ServiceStyle.table}>" +
                                $"<tr>" +
                                    $"<td {ServiceStyle.td}>Вид страховки</td>" +
                                    $"<td {ServiceStyle.td}>Страховая компания</td>" +
                                    (!string.IsNullOrEmpty(Comment) ? $"<td {ServiceStyle.td}>Примечания</td>" : "") +
                                    (Cost != 0 ? $"<td {ServiceStyle.td}>Стоимость</td>" : "") +
                                $"</tr>" +
                                $"<tr>" +
                                    $"<td {ServiceStyle.td}>{InsuranceType?.Name}</td><td>{insuranceCompanyInfo}</td>" +
                                    (!string.IsNullOrEmpty(Comment) ? $"<td {ServiceStyle.td}>{Comment}</td>" : "") +
                                    (Cost != 0 ? $"<td {ServiceStyle.td}>{CostCurrency}</td>" : "") +
                                $"</tr>" +
                            $"</table>";
                return details;
            }
        }
    }

    public class InsuranceType : AppType { }

    public class VisaService : Service
    {
        public VisaService() : base()
        {
            Name = "Виза";
            Sequence = 80;
        }

        [Display(Name = "Вариант визовой поддержки")]
        public VisaType VisaType { get; set; }    // Тип визовой услуги (полная поддержка, ч-з оператора, только подготовка документов)

        [Display(Name = "Вариант визовой поддержки")]
        public Guid? VisaTypeId { get; set; }

        public override string Details
        {
            get
            {
                string details = base.Details;
                details += $" | {VisaType?.Name} | ";
                return details;
            }
        }

        public override string DetailsPrint
        {
            get
            {
                string details = base.DetailsPrint;
                details += $"<table {ServiceStyle.table}>" +
                                $"<tr>" +
                                    $"<td {ServiceStyle.td}>Виза</td>" +
                                    $"<td {ServiceStyle.td}>{VisaType?.Name ?? "Нет"}</td>" +
                                    (!string.IsNullOrEmpty(Comment) ? $"<td {ServiceStyle.td}>{Comment}</td>" : "") +
                                    (Cost != 0 ? ($"<td {ServiceStyle.td}>Стоимость: {CostCurrency} </td>") : "") +
                                $"</tr>" +
                            $"</table>";
                return details;
            }
        }
    }

    public class VisaType : AppType { }

    public class ExcursionService : Service
    {
        public ExcursionService() : base()
        {
            Name = "Экскурсия";
            Sequence = 90;
        }

        public override string Details
        {
            get
            {
                string details = $"{CostCurrency} | Экскурсия - {Name} | {Description} | {Comment} | {DateBeginString} - {TimeBeginString} | {DateEndString} - {TimeEndString}";
                return details;
            }
        }

        public override string DetailsPrint
        {
            get
            {
                string details = base.DetailsPrint;
                details += $"<table {ServiceStyle.table}>" +
                                $"<tr>" +
                                    $"<td {ServiceStyle.td}>Экскурсия</td>" +
                                    $"<td {ServiceStyle.td}>Описание</td>" +
                                    $"<td {ServiceStyle.td}>Даты</td>" +
                                    (!string.IsNullOrEmpty(Comment) ? $"<td {ServiceStyle.td}>Примечания</td>" : "") +
                                    (Cost != 0 ? $"<td {ServiceStyle.td}>Стоимость</td>" : "") +
                                $"</tr>" +
                                $"<tr>" +
                                    $"<td {ServiceStyle.td}>{Name ?? "Нет"}</td>" +
                                    $"<td {ServiceStyle.td}>{Description}</td>" +
                                    $"<td {ServiceStyle.td}>{DateBeginString} {TimeBeginString} - {DateEndString} {TimeEndString}</td>" +
                                    (!string.IsNullOrEmpty(Comment) ? $"<td {ServiceStyle.td}>{Comment}</td>" : "") +
                                    (Cost != 0 ? $"<td {ServiceStyle.td}>{CostCurrency}</td>" : "") +
                                $"</tr>" +
                            $"</table>";
                return details;
            }
        }
    }

    public class AdditionalService : Service
    {
        public AdditionalService() : base()
        {
            Name = "Доп. услуга";
            Sequence = 100;
        }

        public override string Details
        {
            get
            {
                string details = $"{CostCurrency} | Доп. услуга - {Name} | {Description} | {Comment} | {DateBeginString} - {TimeBeginString} | {DateEndString} - {TimeEndString}";
                return details;
            }
        }

        public override string DetailsPrint
        {
            get
            {
                string details = base.DetailsPrint;
                details += $"<table {ServiceStyle.table}>" +
                                $"<tr>" +
                                    $"<td {ServiceStyle.td}>Доп. услуга</td>" +
                                    $"<td {ServiceStyle.td}>Описание</td>" +
                                    $"<td {ServiceStyle.td}>Даты</td>" +
                                    (!string.IsNullOrEmpty(Comment) ? $"<td {ServiceStyle.td}>Примечания</td>" : "") +
                                    (Cost != 0 ? $"<td {ServiceStyle.td}td>Стоимость</td>" : "") +
                                $"</tr>" +
                                $"<tr>" +
                                    $"<td {ServiceStyle.td}>{Name ?? "Нет"}</td>" +
                                    $"<td {ServiceStyle.td}>{Description}</td>" +
                                    $"<td {ServiceStyle.td}>{DateBeginString} {TimeBeginString} - {DateEndString} {TimeEndString}</td>" +
                                    (!string.IsNullOrEmpty(Comment) ? $"< {ServiceStyle.td}td>{Comment}</td>" : "") +
                                    (Cost != 0 ? $"<td {ServiceStyle.td}>{CostCurrency}</td>" : "") +
                                $"</tr>" +
                            $"</table>";
                return details;
            }
        }
    }
}
