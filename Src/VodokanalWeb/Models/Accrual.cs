using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VodokanalWeb.Models
{
    public class Accrual
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Номер начисления")]
        public string AccrualNumber { get; set; } = string.Empty;

        [Display(Name = "ID абонента")]
        public int SubscriberId { get; set; }

        [Display(Name = "Период")]
        public string Period { get; set; } = string.Empty;

        [Display(Name = "ID услуги")]
        public int ServiceId { get; set; }

        [Display(Name = "Сумма")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Display(Name = "Расход")]
        public decimal Consumption { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Срок оплаты")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Статус")]
        public string Status { get; set; } = string.Empty;

        [ForeignKey("SubscriberId")]
        public virtual Subscriber? Subscriber { get; set; }

        [ForeignKey("ServiceId")]
        public virtual Service? Service { get; set; }
    }
}
