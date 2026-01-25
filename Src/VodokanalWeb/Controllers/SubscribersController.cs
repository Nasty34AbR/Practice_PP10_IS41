using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VodokanalWeb.Models;

namespace VodokanalWeb.Controllers
{
    public class SubscribersController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<SubscribersController> _logger;

        public SubscribersController(DatabaseContext context, ILogger<SubscribersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string searchString, string subscriberType, int page = 1)
        {
            try
            {
                var subscribersQuery = _context.Subscribers.AsQueryable();

                if (!string.IsNullOrEmpty(searchString))
                {
                    subscribersQuery = subscribersQuery.Where(s =>
                        s.FullName.Contains(searchString) ||
                        s.Address.Contains(searchString) ||
                        s.AccountNumber.Contains(searchString) ||
                        s.Phone.Contains(searchString));
                }


                if (!string.IsNullOrEmpty(subscriberType) && subscriberType != "Все")
                {
                    if (int.TryParse(subscriberType, out int typeId))
                    {
                        subscribersQuery = subscribersQuery.Where(s => s.SubscriberTypeId == typeId);
                    }
                }

                int pageSize = 10;
                var totalItems = await subscribersQuery.CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                var subscribers = await subscribersQuery
                    .OrderBy(s => s.FullName)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var subscriberTypes = await _context.SubscriberTypes.ToListAsync();

                ViewBag.SearchString = searchString;
                ViewBag.SubscriberType = subscriberType;
                ViewBag.SubscriberTypes = subscriberTypes;
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;
                ViewBag.TotalItems = totalItems;

                return View(subscribers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке списка абонентов");
                TempData["ErrorMessage"] = "Произошла ошибка при загрузке данных";
                return View(new List<Subscriber>());
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscriber = await _context.Subscribers
                .FirstOrDefaultAsync(m => m.Id == id);

            if (subscriber == null)
            {
                return NotFound();
            }

            var subscriberType = await _context.SubscriberTypes
                .FirstOrDefaultAsync(st => st.Id == subscriber.SubscriberTypeId);

            ViewBag.SubscriberTypeName = subscriberType?.Name ?? "Не указан";

            var meters = await _context.Meters
                .Where(m => m.SubscriberId == id)
                .ToListAsync();
            ViewBag.Meters = meters;

            var recentPayments = await _context.Payments
                .Where(p => p.SubscriberId == id)
                .OrderByDescending(p => p.PaymentDate)
                .Take(5)
                .ToListAsync();
            ViewBag.RecentPayments = recentPayments;

            var recentAccruals = await _context.Accruals
                .Where(a => a.SubscriberId == id)
                .OrderByDescending(a => a.CreatedDate)
                .Take(5)
                .ToListAsync();
            ViewBag.RecentAccruals = recentAccruals;

            return View(subscriber);
        }

        public IActionResult Create()
        {
            ViewBag.SubscriberTypes = _context.SubscriberTypes.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountNumber,SubscriberTypeId,FullName,Address,PersonalAccount,INN,KPP,Phone,Email,RegistrationDate,Balance")] Subscriber subscriber)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    subscriber.RegistrationDate = DateTime.Now;

                    _context.Add(subscriber);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Абонент успешно создан";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании абонента");
                ModelState.AddModelError("", "Произошла ошибка при создании абонента");
            }

            ViewBag.SubscriberTypes = _context.SubscriberTypes.ToList();
            return View(subscriber);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscriber = await _context.Subscribers.FindAsync(id);
            if (subscriber == null)
            {
                return NotFound();
            }

            ViewBag.SubscriberTypes = _context.SubscriberTypes.ToList();
            return View(subscriber);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountNumber,SubscriberTypeId,FullName,Address,PersonalAccount,INN,KPP,Phone,Email,RegistrationDate,Balance,UserID")] Subscriber subscriber)
        {
            if (id != subscriber.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subscriber);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Данные абонента успешно обновлены";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubscriberExists(subscriber.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.SubscriberTypes = _context.SubscriberTypes.ToList();
            return View(subscriber);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscriber = await _context.Subscribers
                .FirstOrDefaultAsync(m => m.Id == id);

            if (subscriber == null)
            {
                return NotFound();
            }

            return View(subscriber);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subscriber = await _context.Subscribers.FindAsync(id);
            if (subscriber != null)
            {
                var hasMeters = await _context.Meters.AnyAsync(m => m.SubscriberId == id);
                var hasPayments = await _context.Payments.AnyAsync(p => p.SubscriberId == id);
                var hasAccruals = await _context.Accruals.AnyAsync(a => a.SubscriberId == id);

                if (hasMeters || hasPayments || hasAccruals)
                {
                    TempData["ErrorMessage"] = "Невозможно удалить абонента, так как у него есть связанные данные (счетчики, платежи, начисления)";
                    return RedirectToAction(nameof(Index));
                }

                _context.Subscribers.Remove(subscriber);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Абонент успешно удален";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SubscriberExists(int id)
        {
            return _context.Subscribers.Any(e => e.Id == id);
        }
    }
}
