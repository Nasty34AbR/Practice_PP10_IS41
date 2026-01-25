using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VodokanalWeb.Models
{
    public class Meter
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Серийный номер")]
        public string SerialNumber { get; set; } = string.Empty;

        [Display(Name = "Дата установки")]
        public DateTime InstallationDate { get; set; }

        [Display(Name = "Начальные показания")]
        public decimal InitialReading { get; set; }

        [Display(Name = "ID абонента")]
        public int SubscriberId { get; set; }

        [Display(Name = "Дата последней проверки")]
        public DateTime? LastCheckDate { get; set; }

        [Display(Name = "Дата следующей проверки")]
        public DateTime? NextCheckDate { get; set; }

        [Display(Name = "Активен")]
        public bool IsActive { get; set; }

        [ForeignKey("SubscriberId")]
        public virtual Subscriber? Subscriber { get; set; }
    }
}
