using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(INotificationService notificationService, ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 25;
            var all = (await _notificationService.GetNotificationsAsync()).OrderBy(x => x.NotificationId).ToList();
            var totalPages = Math.Max(1, (int)Math.Ceiling(all.Count / (double)pageSize));
            page = Math.Clamp(page, 1, totalPages);

            var items = all
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewData["Pagination"] = new PaginationViewModel
            {
                CurrentPage = page,
                TotalPages = totalPages,
                ControllerName = "Notification"
            };

            return View(items);
        }

        // Estas acciones devuelven únicamente el contenido del modal, no una vista completa.
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);

            if (notification is null)
            {
                return NotFound("The requested notification was not found.");
            }

            return PartialView("_Details", notification);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Create", new NotificationDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NotificationDTO notification)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Create", notification);
            }

            notification.CreatedAt = DateTime.Now;

            var saved = await _notificationService.SaveAsync(notification);

            if (!saved)
            {
                ViewData["ModalError"] = "The notification could not be created.";
                return PartialView("_Create", notification);
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);

            if (notification is null)
            {
                return NotFound("The requested notification was not found.");
            }

            return PartialView("_Edit", notification);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NotificationDTO notification)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Edit", notification);
            }

            var saved = await _notificationService.SaveAsync(notification);

            if (!saved)
            {
                ViewData["ModalError"] = "The notification could not be updated.";
                return PartialView("_Edit", notification);
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);

            if (notification is null)
            {
                return NotFound("The requested notification was not found.");
            }

            return PartialView("_Delete", notification);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(NotificationDTO notification)
        {
            var deleted = await _notificationService.DeleteAsync(notification.NotificationId);

            if (!deleted)
            {
                ViewData["ModalError"] = "The notification could not be deleted.";
                return PartialView("_Delete", notification);
            }

            return Json(new { success = true });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
