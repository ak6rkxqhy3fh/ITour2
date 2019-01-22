using RSDN;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITour.Models
{
    public class Profit
    {
        public Profit()
        {
            ManualBasicProfit = 0;
            ManualAddProfit = 0;
        }

        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }

        [Display(Name = "Заказ")]
        public Order Order { get; set; }
        [Display(Name = "Заказ")]
        public Guid OrderId { get; set; }

        [Display(Name = "Скрыть")]
        public bool IsHide { get; set; }


        [Display(Name = "Рассчитанное базовое вознаграждение")]
        public decimal? СalculatedBasicProfit => Order?.OrderCost - Order?.OutgoingPaymentsTotal;
        [Display(Name = "Рассчитанное базовое вознаграждение")]
        public string СalculatedBasicProfitCurrency => СalculatedBasicProfit != null ? $"{((decimal)СalculatedBasicProfit).ToString("C")}" : "";
        [Display(Name = "Рассчитанное базовое вознаграждение")]
        public string СalculatedBasicProfitNumeric => СalculatedBasicProfit != null ? $"{((decimal)СalculatedBasicProfit).ToString("N")}" : "";

        [Column(TypeName = "decimal(9, 2)")]
        [Display(Name = "Ручное базовое вознаграждение")]
        public decimal ManualBasicProfit { get; set; }
        [Display(Name = "Ручное базовое вознаграждение")]
        public string ManualBasicProfitCurrency => $"{ManualBasicProfit.ToString("C")}";
        [Display(Name = "Ручное базовое вознаграждение")]
        public string ManualBasicProfitNumeric => $"{ManualBasicProfit.ToString("N")}";

        [Display(Name = "Базовое Вознаграждение")]
        public decimal? BasicProfit => СalculatedBasicProfit + ManualBasicProfit;
        [Display(Name = "Базовое Вознаграждение")]
        public string BasicProfitCurrency => BasicProfit != null ? $"{((decimal)BasicProfit).ToString("C")}" : "";
        [Display(Name = "Базовое Вознаграждение")]
        public string BasicProfitNumeric => BasicProfit != null ? $"{((decimal)BasicProfit).ToString("N")}" : "";


        [Display(Name = "Рассчитанное доп вознаграждение")]
        public decimal? СalculatedAddProfit => Order?.IncomingPaymentsTotal - Order?.OrderCost;
        [Display(Name = "Рассчитанное доп вознаграждение")]
        public string СalculatedAddProfitCurrency => СalculatedAddProfit != null ? $"{((decimal)СalculatedAddProfit).ToString("C")}" : "";
        [Display(Name = "Рассчитанное доп вознаграждение")]
        public string СalculatedAddProfitNumeric => СalculatedAddProfit != null ? $"{((decimal)СalculatedAddProfit).ToString("N")}" : "";

        [Column(TypeName = "decimal(9, 2)")]
        [Display(Name = "Ручное доп вознаграждение")]
        public decimal ManualAddProfit { get; set; }
        [Display(Name = "Ручное доп вознаграждение")]
        public string ManualAddProfitCurrency => $"{ManualAddProfit.ToString("C")}";
        [Display(Name = "Ручное доп вознаграждение")]
        public string ManualAddProfitNumeric => $"{ManualAddProfit.ToString("N")}";

        [Display(Name = "Доп Вознаграждение")]
        public decimal? AddProfit => СalculatedAddProfit + ManualAddProfit;
        [Display(Name = "Доп Вознаграждение")]
        public string AddProfitCurrency => AddProfit != null ? $"{((decimal)AddProfit).ToString("C")}" : "";
        [Display(Name = "Доп Вознаграждение")]
        public string AddProfitNumeric => AddProfit != null ? $"{((decimal)AddProfit).ToString("N")}" : "";

        [Display(Name = "Вознаграждение")]
        public decimal? ProfitTotal => BasicProfit + AddProfit;
        [Display(Name = "Вознаграждение")]
        public string ProfitTotalCurrency => ProfitTotal != null ? $"{((decimal)ProfitTotal).ToString("C")}" : "";
        [Display(Name = "Вознаграждение")]
        public string ProfitTotalNumeric => ProfitTotal != null ? $"{((decimal)ProfitTotal).ToString("N")}" : "";
        [Display(Name = "Вознаграждение")]
        public string ProfitTotalString => ProfitTotal != null ? $"{RusCurrency.Str((double)ProfitTotal)}" : "";        
    }

    // ProfitTotals
    #region
    //public class ProfitTotals
    //{
    //    public ProfitTotals(IList<Order> orders)
    //    {
    //        TotalBasicProfit = orders.Sum(o => o.Profit.BasicProfit);
    //        TotalAddProfit = orders.Sum(o => o.Profit.AddProfit);
    //        TotalProfit = orders.Sum(o => o.Profit.ProfitTotal);

    //    }

    //    [Display(Name = "Итого Базовое вознаграждение")]
    //    public decimal? TotalBasicProfit { get; set; }
    //    [Display(Name = "Итого Базовое Вознаграждение")]
    //    public string TotalBasicProfitCurrency => $"{((decimal)TotalBasicProfit).ToString("C")}";
    //    [Display(Name = "Итого Базовое Вознаграждение")]
    //    public string TotalBasicProfitNumeric => $"{((decimal)TotalBasicProfit).ToString("N")}";

    //    [Display(Name = "Итого Доп Вознаграждение")]
    //    public decimal? TotalAddProfit { get; set; }
    //    [Display(Name = "Итого Доп Вознаграждение")]
    //    public string TotalAddProfitCurrency => $"{((decimal)TotalAddProfit).ToString("C")}";
    //    [Display(Name = "Итого Доп Вознаграждение")]
    //    public string TotalAddProfitNumeric => $"{((decimal)TotalAddProfit).ToString("N")}";

    //    [Display(Name = "Итого Вознаграждение ")]
    //    public decimal? TotalProfit { get; set; }
    //    [Display(Name = "Итого Вознаграждение")]
    //    public string TotalProfitCurrency => $"{((decimal)TotalProfit).ToString("C")}";
    //    [Display(Name = "Итого Вознаграждение")]
    //    public string TotalProfitNumeric => $"{((decimal)TotalProfit).ToString("N")}";
    //}
    #endregion
}
