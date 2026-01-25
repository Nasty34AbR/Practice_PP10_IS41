using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VodokanalWeb.Models
{
    public class MeterReading
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "ID счетчика")]
        public int MeterId { get; set; }

        [Display(Name = "Дата снятия показаний")]
        public DateTime ReadingDate { get; set; }

        [Display(Name = "Текущие показания")]
        public decimal CurrentReading { get; set; }

        [Display(Name = "Расход")]
        public decimal Consumption { get; set; }

        [Display(Name = "Подтверждено")]
        public bool IsConfirmed { get; set; }

        [Display(Name = "Подтверждено кем")]
        public string? ConfirmedBy { get; set; }

        [Display(Name = "Дата подтверждения")]
        public DateTime? ConfirmationDate { get; set; }

        [ForeignKey("MeterId")]
        public virtual Meter? Meter { get; set; }
    }
}
