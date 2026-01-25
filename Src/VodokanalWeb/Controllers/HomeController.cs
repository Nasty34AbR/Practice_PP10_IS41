using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VodokanalWeb.Models;
using System.Diagnostics;

namespace VodokanalWeb.Controllers
{
    public class HomeController(ILogger<HomeController> logger, DatabaseContext context) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly DatabaseContext _context = context;

        public IActionResult Index()
        {
            var viewModel = new HomeViewModel
            {
                RecentPayments = GetRecentPayments(),
                UpcomingPayments = GetUpcomingPayments()
            };

            return View(viewModel);
        }
        public IActionResult About()  
        {
            return View();
        }

        public IActionResult Contacts() 
        {
            return View();
        }


     
        private List<RecentPayment> GetRecentPayments()
        {
            try
            {
                
                if (_context.Payments != null && _context.Payments.Any())
                {
                    var recentPayments = _context.Payments
                        .Include(p => p.Subscriber)
                        .OrderByDescending(p => p.PaymentDate)
                        .Take(10)
                        .Select(p => new RecentPayment
                        {
                            PaymentNumber = p.PaymentNumber,
                            PaymentDate = p.PaymentDate,
                            SubscriberName = p.Subscriber != null ? p.Subscriber.FullName : "Неизвестный абонент",
                            Amount = p.Amount,
                            Status = !string.IsNullOrEmpty(p.ConfirmedBy) ? "Подтвержден" : "Ожидает подтверждения",
                            PaymentMethod = p.PaymentMethod
                        })
                        .ToList();

                    if (recentPayments.Count != 0)
                    {
                        return recentPayments;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении последних платежей");
            }

            return GetSampleRecentPayments();
        }

        private List<UpcomingPayment> GetUpcomingPayments()
        {
            try
            {
                if (_context.Accruals != null && _context.Accruals.Any())
                {
                    var upcomingPayments = _context.Accruals
                        .Include(a => a.Subscriber)
                        .Include(a => a.Service)
                        .Where(a => a.Status != "Оплачено" && a.DueDate >= DateTime.Today)
                        .OrderBy(a => a.DueDate)
                        .Take(10)
                        .Select(a => new UpcomingPayment
                        {
                            AccrualNumber = a.AccrualNumber,
                            SubscriberName = a.Subscriber != null ? a.Subscriber.FullName : "Неизвестный абонент",
                            DueDate = a.DueDate,
                            Amount = a.Amount,
                            ServiceName = a.Service != null ? a.Service.ServiceName : "Неизвестная услуга",
                            DaysUntilDue = (int)(a.DueDate - DateTime.Today).TotalDays
                        })
                        .ToList();

                    if (upcomingPayments.Count != 0)
                    {
                        return upcomingPayments;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении ближайших платежей");
            }

            return GetSampleUpcomingPayments();
        }

        private static List<RecentPayment> GetSampleRecentPayments()
        {
            return
            [
                new()
                {
                    PaymentNumber = "ПЛТ-2024-01-001",
                    PaymentDate = DateTime.Now.AddDays(-2),
                    SubscriberName = "Иванов Иван Иванович",
                    Amount = 227.50m,
                    Status = "Подтвержден",
                    PaymentMethod = "СБП"
                },
                new()
                {
                    PaymentNumber = "ПЛТ-2023-12-002",
                    PaymentDate = DateTime.Now.AddDays(-15),
                    SubscriberName = "ООО \"Вода+\"",
                    Amount = 500.00m,
                    Status = "Подтвержден",
                    PaymentMethod = "Наличные"
                },
                new()
                {
                    PaymentNumber = "ПЛТ-2023-12-001",
                    PaymentDate = DateTime.Now.AddDays(-18),
                    SubscriberName = "Иванов Иван Иванович",
                    Amount = 773.65m,
                    Status = "Подтвержден",
                    PaymentMethod = "Банковская карта"
                }
            ];
        }

        private static List<UpcomingPayment> GetSampleUpcomingPayments()
        {
            return
            [
                new()
                {
                    AccrualNumber = "НАЧ-2024-01-001",
                    SubscriberName = "Петрова Мария Сергеевна",
                    DueDate = DateTime.Today.AddDays(3),
                    Amount = 227.50m,
                    ServiceName = "Холодное водоснабжение",
                    DaysUntilDue = 3
                },
                new()
                {
                    AccrualNumber = "НАЧ-2023-12-003",
                    SubscriberName = "ООО \"Вода+\"",
                    DueDate = DateTime.Today.AddDays(5),
                    Amount = 1160.25m,
                    ServiceName = "Холодное водоснабжение",
                    DaysUntilDue = 5
                }
            ];
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
