using System;
using System.ComponentModel.DataAnnotations;

namespace ITour.Services.Options
{
    public class AppOptions
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }

        [Display(Name = "Платежи")]
        public bool IsUsePayments { get; set; }

        [Display(Name = "Комиссия")]
        public bool IsUseCommissionReport { get; set; }

        [Display(Name = "Вознаграждение")]
        public bool IsUseProfits { get; set; }
    }
}
