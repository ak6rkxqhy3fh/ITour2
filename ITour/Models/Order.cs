using RSDN;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ITour.Models
{
    public class Order
    {
        public Order()
        {
            CreateDate = DateTime.Now;
            DatePrint = DateTime.Today;
            Touroperators = new List<OrderTouroperatorCompany>();
            Tourists = new List<OrderTourist>();
            Services = new List<Service>();
            IncomingPayments = new List<IncomingPayment>();
            OutgoingPayments = new List<OutgoingPayment>();
        }

        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        [Display(Name = "Удален")]
        public bool IsDeleted { get; set; }

        [Display(Name = "№ ")]
        public int? Number { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Дата")]
        public DateTime? CreateDate { get; set; }
        [Display(Name = "Дата")]
        public string CreateDateString => ((DateTime)CreateDate).ToShortDateString();

        [DataType(DataType.Date)]
        [Display(Name = "Дата в док")]
        public DateTime? DatePrint { get; set; }
        public string DatePrintString => $"{((DatePrint != null) ? ((DateTime)DatePrint).ToShortDateString() : "")}";

        [DataType(DataType.Date)]
        [Display(Name = "Дата начала")]
        public DateTime? DateBegin { get; set; }
        public string DateBeginString => $"{((DateBegin != null) ? ((DateTime)DateBegin).ToShortDateString() : "")}";

        [DataType(DataType.Date)]
        [Display(Name = "Дата окончания")]
        public DateTime? DateEnd { get; set; }
        public string DateEndString => $"{((DateEnd != null) ? ((DateTime)DateEnd).ToShortDateString() : "")}";

        [Display(Name = "Количество дней")]
        public int? CountDays { get; set; }

        [Display(Name = "Количество ночей")]
        public int? CountNights { get; set; }

        [Display(Name = "Статус")]
        public OrderStatus OrderStatus { get; set; }
        [Display(Name = "Статус")]
        public Guid? OrderStatusId { get; set; }

        [Display(Name = "Подтверждение")]
        public string ReservationNumber { get; set; }

        [Display(Name = "Агентство")]
        public AgencyCompany AgencyCompany { get; set; }
        //[Required(ErrorMessage = "Агентство нужно выбрать!")]
        [Display(Name = "Агентство")]
        public Guid? AgencyCompanyId { get; set; }

        [Display(Name = "Менеджер")]
        public Manager Manager { get; set; }
        [Display(Name = "Менеджер")]
        public Guid? ManagerId { get; set; }

        [Display(Name = "Заказчик")]
        public Customer Customer { get; set; }
        [Display(Name = "Заказчик")]
        public Guid? CustomerId { get; set; }

        [Display(Name = "Туроператор")]
        public TouroperatorBrand TouroperatorBrand { get; set; }
        [Display(Name = "Туроператор")]
        public Guid? TouroperatorBrandId { get; set; }

        [Display(Name = "Юр. лица ТО")]
        public List<OrderTouroperatorCompany> Touroperators { get; set; }

        [Display(Name = "Туристы")]
        public List<OrderTourist> Tourists { get; set; }
        [Display(Name = "Туристы")]
        public string TouristsString { get; set; }

        [Display(Name = "Кол-во туристов")]
        public int? TouristsCount { get; set; }

        [Display(Name = "Услуги")]
        public List<Service> Services { get; set; }

        [Display(Name = "Платежи от клиентов")]
        public List<IncomingPayment> IncomingPayments { get; set; }

        [Display(Name = "Платежи поставщикам")]
        public List<OutgoingPayment> OutgoingPayments { get; set; }

        [Display(Name = "Примечания")]
        public string Comment { get; set; }

        [Display(Name = "Стоимость Заказа")]
        public decimal OrderCost => (Services?.Count > 0) ? (Services.Sum(s => (decimal)s.Cost)) : 0;
        [Display(Name = "Стоимость Заказа")]
        public string OrderCostCurrency => $"{OrderCost.ToString("C")}";
        [Display(Name = "Стоимость Заказа")]
        public string OrderCostNumeric => $"{OrderCost.ToString("N")}";
        [Display(Name = "Стоимость Заказа")]
        public string OrderCostString => $"{RusCurrency.Str((double)OrderCost)}";

        [Display(Name = "Итого входящие платежи")]
        public decimal IncomingPaymentsTotal => (IncomingPayments?.Count > 0) ? (IncomingPayments.Sum(ip => (decimal)ip.PaymentAmount)) : 0;
        [Display(Name = "Итого входящие платежи")]
        public string IncomingPaymentsTotalCurrency => $"{IncomingPaymentsTotal.ToString("C")}";
        [Display(Name = "Итого входящие платежи")]
        public string IncomingPaymentsTotalNumeric => $"{IncomingPaymentsTotal.ToString("N")}";
        [Display(Name = "Итого входящие платежи")]
        public string IncomingPaymentsTotaString => $"{RusCurrency.Str((double)IncomingPaymentsTotal)}";

        [Display(Name = "Комиссия банка")]
        public decimal IncomingPaymentsBankCommissionTotal => (IncomingPayments?.Count > 0) ? (IncomingPayments.Sum(ip => (decimal)ip.BankCommission)) : 0;
        [Display(Name = "Комиссия банка")]
        public string IncomingPaymentsBankCommissionTotalCurrency => $"{IncomingPaymentsBankCommissionTotal.ToString("C")}";
        [Display(Name = "Комиссия банка")]
        public string IncomingPaymentsBankCommissionTotalNumeric => $"{IncomingPaymentsBankCommissionTotal.ToString("N")}";

        [Display(Name = "Итого исходящие платежи")]
        public decimal OutgoingPaymentsTotal => (OutgoingPayments?.Count > 0) ? (OutgoingPayments.Sum(ip => (decimal)ip.PaymentAmount)) : 0;
        [Display(Name = "Итого исходящие платежи")]
        public string OutgoingPaymentsTotalCurrency => $"{OutgoingPaymentsTotal.ToString("C")}";
        [Display(Name = "Итого исходящие платежи")]
        public string OutgoingPaymentsTotalNumeric => $"{OutgoingPaymentsTotal.ToString("N")}";

        [Display(Name = "Задолжность заказчика")]
        public decimal CustomerDebt => OrderCost - IncomingPaymentsTotal;
        [Display(Name = "Задолжность заказчика")]
        public string CustomerDebtCurrency => $"{CustomerDebt.ToString("C")}";
        [Display(Name = "Задолжность заказчика")]
        public string CustomerDebtNumeric => $"{CustomerDebt.ToString("N")}";

        [Display(Name = "Комиссия")]
        public decimal OrderCommission => IncomingPaymentsTotal - OutgoingPaymentsTotal - IncomingPaymentsBankCommissionTotal;
        [Display(Name = "Комиссия")]
        public string OrderCommissionCurrency => $"{OrderCommission.ToString("C")}";
        [Display(Name = "Комиссия")]
        public string OrderCommissionNumeric => $"{OrderCommission.ToString("N")}";

        [DataType(DataType.Date)]
        [Display(Name = "Дата комиссии")]
        public DateTime? OrderCommissionDate => IncomingPayments.Where(ip => ip.PaymentDate.HasValue).Any() ?
            IncomingPayments?.Where(ip => ip.PaymentDate.HasValue).Max(ip => (DateTime)ip.PaymentDate) : null;
        public string OrderCommissionDateString => $"{((OrderCommissionDate != null) ? ((DateTime)OrderCommissionDate).ToShortDateString() : "")}";

        public Profit Profit { get; set; }
    }

    public class OrderNumber
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
        public int Value { get; set; }
    }

    public class OrderTouroperatorCompany
    {
        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        [Display(Name = "Удален")]
        public bool IsDeleted { get; set; }

        public Order Order { get; set; }
        public Guid OrderId { get; set; }

        public TouroperatorCompany TouroperatorCompany { get; set; }
        public Guid TouroperatorCompanyId { get; set; }
    }

    public class OrderTourist
    {
        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        [Display(Name = "Удален")]
        public bool IsDeleted { get; set; }

        public Order Order { get; set; }
        public Guid OrderId { get; set; }

        public Person Person { get; set; }
        public Guid PersonId { get; set; }
    }

    public class OrderStatus : AppType { }
}
