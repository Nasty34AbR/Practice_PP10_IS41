using System.ComponentModel.DataAnnotations;

namespace VodokanalWeb.Models
{
    public class SystemLog
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Дата и время")]
        public DateTime LogDate { get; set; }

        [Display(Name = "Уровень")]
        public string LogLevel { get; set; } = string.Empty;

        [Display(Name = "Источник")]
        public string Source { get; set; } = string.Empty;

        [Display(Name = "Сообщение")]
        public string Message { get; set; } = string.Empty;

        [Display(Name = "ID пользователя")]
        public int? UserID { get; set; }

        [Display(Name = "IP-адрес")]
        public string? IPAddress { get; set; }
    }
}
