using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITour.Models
{
    public abstract class ApplicationType
    {
        public Guid Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Очередность")]
        public int Sequence { get; set; }
    }

    public class AppType : ApplicationType
    {
        public Guid? TenantId { get; set; }

        [Display(Name = "Удален")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Системный")]
        public bool IsSystem { get; set; }   // Является ли системным - удаление запрещено
    }
}
