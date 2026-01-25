using System.ComponentModel.DataAnnotations;

namespace VodokanalWeb.Models
{
    public class Subscriber
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Номер счета")]
        public string AccountNumber { get; set; } = string.Empty;

        [Display(Name = "Тип абонента")]
        public int SubscriberTypeId { get; set; }

        [Display(Name = "ФИО/Название")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Адрес")]
        public string Address { get; set; } = string.Empty;

        [Display(Name = "Лицевой счет")]
        public string? PersonalAccount { get; set; }

        [Display(Name = "ИНН")]
        public string? INN { get; set; }

        [Display(Name = "КПП")]
        public string? KPP { get; set; }

        [Display(Name = "Телефон")]
        public string? Phone { get; set; }

        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Display(Name = "Дата регистрации")]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Баланс")]
        public decimal Balance { get; set; }

        [Display(Name = "ID пользователя")]
        public int? UserID { get; set; }
    }
}
