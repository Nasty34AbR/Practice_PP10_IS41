using System.ComponentModel.DataAnnotations;

namespace WaterPaymentSystem.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [MaxLength(50)]
        public string ?Period { get; set; } // Например: "01.2024"

        [MaxLength(20)]
        public string ?PaymentMethod { get; set; }

        [MaxLength(500)]
        public string ?Description { get; set; }

        // Навигационное свойство
        public Client ?Client { get; set; }
    }
}
