using System.ComponentModel.DataAnnotations;

namespace VodokanalWeb.Models
{
    public class SubscriberType
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Название типа")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Описание")]
        public string? Description { get; set; }
    }
}
