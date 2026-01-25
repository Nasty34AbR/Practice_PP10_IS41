using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VodokanalWeb.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Номер платежа")]
        public string PaymentNumber { get; set; } = string.Empty;

        [Display(Name = "ID абонента")]
        public int SubscriberId { get; set; }

        [Display(Name = "Дата платежа")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Сумма")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Display(Name = "Способ оплаты")]
        public string PaymentMethod { get; set; } = string.Empty;

        [Display(Name = "Назначение")]
        public string Purpose { get; set; } = string.Empty;

        [Display(Name = "ID начисления")]
        public int? AccrualId { get; set; }

        [Display(Name = "Подтверждено")]
        public string? ConfirmedBy { get; set; }

        [Display(Name = "Дата подтверждения")]
        public DateTime? ConfirmationDate { get; set; }

        [ForeignKey("SubscriberId")]
        public virtual Subscriber? Subscriber { get; set; }

        [ForeignKey("AccrualId")]
        public virtual Accrual? Accrual { get; set; }
    }
}
