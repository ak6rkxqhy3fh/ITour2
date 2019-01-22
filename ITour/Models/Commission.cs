using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ITour.Models
{
    public class Commission
    {
        public bool IsDeleted { get; set; }

        public Guid? TenantId { get; set; }

        public Guid? OrderId { get; set; }

        [Display(Name = "Заказ")]
        public int? OrderNumber { get; set; }

        [Display(Name = "Агентство")]
        public string AgencyCompanyName { get; set; }
        public Guid? AgencyCompanyId { get; set; }

        public Guid? AgencyOfficeId { get; set; }

        [Display(Name = "Менеджер")]
        public string ManagerName { get; set; }
        public Guid? ManagerId { get; set; }

        [Display(Name = "Заказчик")]
        public string CustomerName { get; set; }
        public string CustomerCompanyName { get; set; }

        [Column(TypeName = "decimal(9, 2)"), DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Стоимость Заказа")]
        public decimal? OrderCost { get; set; }
        public string OrderCostNumeric => $"{OrderCost?.ToString("N")}";

        [Column(TypeName = "decimal(9, 2)"), DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Входящие платежи")]
        public decimal? IncomingPaymentsTotal { get; set; }
        public string IncomingPaymentsTotalNumeric => $"{IncomingPaymentsTotal?.ToString("N")}";

        [Column(TypeName = "decimal(9, 2)"), DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Долг Заказчика")]
        public decimal? CustomerDebt { get; set; }
        public string CustomerDebtNumeric => $"{CustomerDebt?.ToString("N")}";

        [Column(TypeName = "decimal(9, 2)"), DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Коммисия банка")]
        public decimal? BankCommissionTotal { get; set; }
        public string BankCommissionTotalNumeric => $"{BankCommissionTotal?.ToString("N")}";

        [Column(TypeName = "decimal(9, 2)"), DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Исходящие платежи")]
        public decimal? OutgoingPaymentsTotal { get; set; }
        public string OutgoingPaymentsTotalNumeric => $"{OutgoingPaymentsTotal?.ToString("N")}";

        [Column(TypeName = "decimal(9, 2)"), DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Комиссия")]
        public decimal? OrderCommission { get; set; }
        public string OrderCommissionNumeric => $"{(OrderCommission)?.ToString("N")}";

        [Display(Name = "Дата комиссии"), DataType(DataType.Date)]
        public DateTime? OrderCommissionDate { get; set; }

        [Display(Name = "Туроператор")]
        public string Touroperators { get; set; }

        [Display(Name = "Партнер")]
        public string Partners { get; set; }        

        public static string CommissionQuery =>
             @"SELECT 
	            _Order.IsDeleted, _Order.TenantId AS TenantId, _Order.Id AS OrderId, _Order.Number AS OrderNumber, AgencyCompany.Name AS AgencyCompanyName, AgencyCompany.Id AS AgencyCompanyId, Manager.AgencyOfficeId AS AgencyOfficeId,
	            CONCAT([Manager.Person].Surname, ' ', SUBSTRING([Manager.Person].Firstname, 1, 1), '. ',  SUBSTRING([Manager.Person].Middlename, 1, 1), '.') AS ManagerName, Manager.Id AS ManagerId,
	            CONCAT([Customer.Person].Surname, ' ', [Customer.Person].Firstname, ' ',  [Customer.Person].Middlename)   AS CustomerName,
	            CustomerCompany.Name  AS CustomerCompanyName,
                '' AS Touroperators,
                '' AS Partners,
	            SUM(_Money.Cost) AS OrderCost, 
	            SUM(_Money.IncomingPaymentAmount) AS IncomingPaymentsTotal, 
	            SUM(_Money.Cost) - SUM(_Money.IncomingPaymentAmount) AS CustomerDebt,
	            SUM(_Money.BankCommission) AS BankCommissionTotal,
	            SUM(_Money.OutgoingPaymentAmount) AS OutgoingPaymentsTotal,
	            SUM(_Money.IncomingPaymentAmount) - SUM(_Money.OutgoingPaymentAmount) - SUM(_Money.BankCommission) AS OrderCommission,
	            MAX(_Money.PaymentDate) AS OrderCommissionDate
                FROM Orders AS _Order
                LEFT JOIN
                (
                    SELECT IP.OrderId OrderId, IP.PaymentDate AS PaymentDate, IP.PaymentAmount AS IncomingPaymentAmount, IP.BankCommission, 0 AS OutgoingPaymentAmount, 0 AS Cost 
                    FROM  PaymentsIncoming AS IP
	                    UNION ALL
                    SELECT OP.OrderId OrderId, NULL AS PaymentDate, 0 AS IncomingPaymentAmount, 0 AS BankCommission, OP.PaymentAmount AS OutgoingPaymentAmount, 0 AS Cost 
                    FROM PaymentsOutgoing AS OP 
	                    UNION ALL
                    SELECT S.OrderId OrderId, NULL AS PaymentDate, 0 AS IncomingPaymentAmount, 0 AS BankCommission, 0 AS OutgoingPaymentAmount , S.Cost AS Cost 
                    FROM Services AS S 
                ) AS _Money ON _Order.Id = _Money.OrderId				
                LEFT JOIN [dbo].[Companies] AS AgencyCompany ON _Order.AgencyCompanyId = AgencyCompany.Id
                LEFT JOIN [dbo].[Managers] AS Manager ON _Order.ManagerId = Manager.Id
                LEFT JOIN [dbo].[People] AS [Manager.Person] ON Manager.PersonId = [Manager.Person].Id
                LEFT JOIN [dbo].[Customers] AS Customer ON _Order.CustomerId = Customer.Id
                LEFT JOIN [dbo].[People] AS [Customer.Person] ON Customer.PersonId = [Customer.Person].Id
                LEFT JOIN [dbo].[Companies] AS CustomerCompany ON Customer.CustomerCompanyId = CustomerCompany.Id
                GROUP BY _Order.IsDeleted, _Order.TenantId, _Order.Id, _Order.Number, AgencyCompany.Name, AgencyCompany.Id, Manager.AgencyOfficeId, [Manager.Person].Surname, [Manager.Person].Firstname,[Manager.Person].Middlename, Manager.Id,
	            [Customer.Person].Surname, [Customer.Person].Firstname, [Customer.Person].Middlename, CustomerCompany.Name 
                ";
    }

    public class CommissionTotals
    {
        public CommissionTotals(IList<Commission> commissions)
        {
            OrderCost = commissions.Sum(c => c.OrderCost);
            IncomingPaymentsTotal = commissions.Sum(c => c.IncomingPaymentsTotal);
            CustomerDebt = commissions.Sum(c => c.CustomerDebt);
            BankCommissionTotal = commissions.Sum(c => c.BankCommissionTotal);
            OutgoingPaymentsTotal = commissions.Sum(c => c.OutgoingPaymentsTotal);
            OrderCommission = commissions.Sum(c => c.OrderCommission);
        }

        [Display(Name = "Всего Стоимость Заказа")]
        public decimal? OrderCost { get; set; }
        public string OrderCostNumeric => $"{OrderCost?.ToString("N")}";
        [Display(Name = "Всего Входящие платежи")]
        public decimal? IncomingPaymentsTotal { get; set; }
        public string IncomingPaymentsTotalNumeric => $"{IncomingPaymentsTotal?.ToString("N")}";
        [Display(Name = "Всего Долг Заказчика")]
        public decimal? CustomerDebt { get; set; }
        public string CustomerDebtNumeric => $"{CustomerDebt?.ToString("N")}";
        [Display(Name = "Всего Коммисия банка")]
        public decimal? BankCommissionTotal { get; set; }
        public string BankCommissionTotalNumeric => $"{BankCommissionTotal?.ToString("N")}";
        [Display(Name = "Всего Исходящие платежи")]
        public decimal? OutgoingPaymentsTotal { get; set; }
        public string OutgoingPaymentsTotalNumeric => $"{OutgoingPaymentsTotal?.ToString("N")}";
        [Display(Name = "Всего Комиссия")]
        public decimal? OrderCommission { get; set; }
        public string OrderCommissionNumeric => $"{(OrderCommission)?.ToString("N")}";
    }    
}
