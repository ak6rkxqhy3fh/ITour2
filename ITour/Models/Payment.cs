using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ITour.Models
{
    public abstract class Payment
    {
        public Payment()
        {
            PaymentDate = DateTime.Today;
            PaymentAmount = 0;
        }
        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        [Display(Name = "Удален")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Заказ")]
        public Order Order { get; set; }
        [Display(Name = "Заказ")]
        public Guid OrderId { get; set; }

        [Display(Name = "Тип платежа")]                 // Предоплата, доплата, полная оплата
        public PaymentType PaymentType { get; set; }
        [Display(Name = "Тип платежа")]
        public Guid? PaymentTypeId { get; set; }

        [Display(Name = "Форма платежа")]               // Наличные, карта, безнал, сертификат
        public PaymentForm PaymentForm { get; set; }
        [Display(Name = "Форма платежа")]
        public Guid? PaymentFormId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата платежа")]
        public DateTime? PaymentDate { get; set; }
        public string PaymentDateString => $"{((PaymentDate != null) ? ((DateTime)PaymentDate).ToShortDateString() : "")}";

        [Column(TypeName = "decimal(9, 2)")/*, DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C}")*/]
        [Display(Name = "Сумма платежа")]
        public decimal? PaymentAmount { get; set; }
        public string PaymentAmountCurrency => PaymentAmount != null ? $"{((decimal)PaymentAmount).ToString("C")}" : "";
        public string PaymentAmountNumeric => PaymentAmount != null ? $"{((decimal)PaymentAmount).ToString("N")}" : "";

        [Display(Name = "Примечания")]
        public string Comment { get; set; }
    }

    public class PaymentTotals
    {
        public PaymentTotals(IList<Order> order)
        {
            TotalOrdersCost = order.Sum(o => o.OrderCost);
            TotalOrdersComission = order.Sum(o => o.OrderCommission);
            TotalIncomingPayments = order.Sum(o => o.IncomingPaymentsTotal);
            TotalCustomerDebt = order.Sum(o => o.CustomerDebt);
            TotalOutgoingPayments = order.Sum(o => o.OutgoingPaymentsTotal);
        }

        public decimal TotalOrdersCost { get; set; }
        [Display(Name = "Всего стоимость")]
        public string TotalOrdersCostCurency => $"{TotalOrdersCost.ToString("C")}";
        [Display(Name = "Всего стоимость")]
        public string TotalOrdersCostNumeric     => $"{TotalOrdersCost.ToString("N")}";

        public decimal TotalOrdersComission { get; set; }
        [Display(Name = "Всего комиссия")]
        public string TotalOrdersComissionCurency => $"{TotalOrdersComission.ToString("C")}";
        [Display(Name = "Всего комиссия")]
        public string TotalOrdersComissionNumeric => $"{TotalOrdersComission.ToString("N")}";


        public decimal TotalIncomingPayments { get; set; }
        [Display(Name = "Всего Входящие платежи")]
        public string TotalIncomingPaymentsCurrency => $"{TotalIncomingPayments.ToString("C")}";
        [Display(Name = "Всего Входящие платежи")]
        public string TotalIncomingPaymentsNumeric => $"{TotalIncomingPayments.ToString("N")}";

        public decimal TotalOutgoingPayments { get; set; }
        [Display(Name = "Всего Исходящие платежи")]
        public string TotalOutgoingPaymentsCurrency => $"{TotalOutgoingPayments.ToString("C")}";
        [Display(Name = "Всего Исходящие платежи")]
        public string TotalOutgoingPaymentsNumeric => $"{TotalOutgoingPayments.ToString("N")}";

        public decimal TotalCustomerDebt { get; set; }
        [Display(Name = "Всего Задолжность ")]
        public string TotalCustomerDebtCurrency => $"{TotalCustomerDebt.ToString("C")}";
        [Display(Name = "Всего Задолжность ")]
        public string TotalCustomerDebtNumeric => $"{TotalCustomerDebt.ToString("N")}";
    }

    public class PaymentType : AppType { } // Тип платежа - Предоплата, доплата, полная оплата

    public class PaymentForm : AppType { } // Форма платежа - Наличные, карта, безнал, сертификат

    [Table("PaymentsIncoming")]
    public class IncomingPayment : Payment
    {
        public IncomingPayment() : base()
        {
            BankCommission = 0;
        }
        [Display(Name = "Квитанция")]
        public string ReceiptNumber { get; set; }

        [Column(TypeName = "decimal(9, 2)")/*, DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C}")*/]
        [Display(Name = "Комиссия банка")]
        public decimal? BankCommission { get; set; }
    }

    [Table("PaymentsOutgoing")]
    public class OutgoingPayment : Payment
    {
        [Display(Name = "Номер счета")]
        public string InvoiceNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата счета")]
        public DateTime? InvoiceDate { get; set; }

        [Display(Name = "Платежное поручение")]
        public string PaymentOrder { get; set; }

        [Display(Name = "Партнер")]
        public PartnerCompany PartnerCompany { get; set; }

        [Display(Name = "Партнер")]
        public Guid? PartnerCompanyId { get; set; }
    }

}
