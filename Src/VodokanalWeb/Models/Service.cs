using System.ComponentModel.DataAnnotations;

namespace VodokanalWeb.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Код услуги")]
        public string ServiceCode { get; set; } = string.Empty;

        [Display(Name = "Название услуги")]
        public string ServiceName { get; set; } = string.Empty;

        [Display(Name = "Описание")]
        public string? Description { get; set; }

        [Display(Name = "Единица измерения")]
        public string Unit { get; set; } = string.Empty;

        [Display(Name = "Тариф")]
        public decimal Rate { get; set; }

        [Display(Name = "Активна")]
        public bool IsActive { get; set; }
    }
}
