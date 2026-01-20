using System.ComponentModel.DataAnnotations;

namespace WaterPaymentSystem.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string ?Username { get; set; }

        [Required]
        public string ?Password { get; set; }

        [Required]
        [MaxLength(100)]
        public string ?Email { get; set; }

        [MaxLength(50)]
        public string Role { get; set; } = "User";

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
