using System.ComponentModel.DataAnnotations;

namespace WaterPaymentSystem.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string ?FullName { get; set; }

        [MaxLength(20)]
        public string ?Phone { get; set; }

        [MaxLength(300)]
        public string ?Address { get; set; }

        [Required]
        [MaxLength(20)]
        public string ?ClientType { get; set; } // "Физическое лицо" или "Юридическое лицо"

        [MaxLength(20)]
        public string ?INN { get; set; } // ИНН для юр. лиц

        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}
