using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class UserActionController : Controller
    {
        private readonly IUserActionService _userActionService;
        private readonly ILogger<UserActionController> _logger;

        public UserActionController(IUserActionService userActionService, ILogger<UserActionController> logger)
        {
            _userActionService = userActionService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 25;
            var all = (await _userActionService.GetUserActionsAsync()).OrderBy(x => x.UserActionId).ToList();
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
                ControllerName = "UserAction"
            };

            return View(items);
        }

        // Estas acciones devuelven únicamente el contenido del modal, no una vista completa.
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userAction = await _userActionService.GetUserActionByIdAsync(id);

            if (userAction is null)
            {
                return NotFound("The requested user action was not found.");
            }

            return PartialView("_Details", userAction);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Create", new UserActionDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserActionDTO userAction)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Create", userAction);
            }

            var saved = await _userActionService.SaveAsync(userAction);

            if (!saved)
            {
                ViewData["ModalError"] = "The user action could not be created.";
                return PartialView("_Create", userAction);
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userAction = await _userActionService.GetUserActionByIdAsync(id);

            if (userAction is null)
            {
                return NotFound("The requested user action was not found.");
            }

            return PartialView("_Edit", userAction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserActionDTO userAction)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Edit", userAction);
            }

            var saved = await _userActionService.SaveAsync(userAction);

            if (!saved)
            {
                ViewData["ModalError"] = "The user action could not be updated.";
                return PartialView("_Edit", userAction);
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userAction = await _userActionService.GetUserActionByIdAsync(id);

            if (userAction is null)
            {
                return NotFound("The requested user action was not found.");
            }

            return PartialView("_Delete", userAction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(UserActionDTO userAction)
        {
            var deleted = await _userActionService.DeleteAsync(userAction.UserActionId);

            if (!deleted)
            {
                ViewData["ModalError"] = "The user action could not be deleted.";
                return PartialView("_Delete", userAction);
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
