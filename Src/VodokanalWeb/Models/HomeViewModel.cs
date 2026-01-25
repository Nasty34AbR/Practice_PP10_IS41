using System.ComponentModel.DataAnnotations;

namespace VodokanalWeb.Models
{
    public class HomeViewModel
    {
        public List<RecentPayment> RecentPayments { get; set; } = [];
        public List<UpcomingPayment> UpcomingPayments { get; set; } = [];
    }

    public class RecentPayment
    {
        [Display(Name = "Номер платежа")]
        public string PaymentNumber { get; set; } = string.Empty;

        [Display(Name = "Дата платежа")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Абонент")]
        public string SubscriberName { get; set; } = string.Empty;

        [Display(Name = "Сумма")]
        public decimal Amount { get; set; }

        [Display(Name = "Статус")]
        public string Status { get; set; } = string.Empty;

        [Display(Name = "Способ оплаты")]
        public string PaymentMethod { get; set; } = string.Empty;
    }

    public class UpcomingPayment
    {
        [Display(Name = "Номер начисления")]
        public string AccrualNumber { get; set; } = string.Empty;

        [Display(Name = "Абонент")]
        public string SubscriberName { get; set; } = string.Empty;

        [Display(Name = "Срок оплаты")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Сумма")]
        public decimal Amount { get; set; }

        [Display(Name = "Услуга")]
        public string ServiceName { get; set; } = string.Empty;

        [Display(Name = "Дней до оплаты")]
        public int DaysUntilDue { get; set; }
    }
}
