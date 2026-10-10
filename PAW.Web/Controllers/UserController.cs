using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 25;
            var allUsers = (await _userService.GetUsersAsync())
                .OrderBy(user => user.UserId)
                .ToList();

            var totalPages = Math.Max(1,
                (int)Math.Ceiling(allUsers.Count / (double)pageSize));
            page = Math.Clamp(page, 1, totalPages);

            var users = allUsers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewData["Pagination"] = new PaginationViewModel
            {
                CurrentPage = page,
                TotalPages = totalPages,
                ControllerName = "User"
            };

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user is null)
            {
                return NotFound("The requested user was not found.");
            }

            return PartialView("_Details", user);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Create", new UserDTO { IsActive = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserDTO user)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Create", user);
            }

            var saved = await _userService.SaveAsync(user);

            if (!saved)
            {
                ViewData["ModalError"] = "The user could not be created.";
                return PartialView("_Create", user);
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user is null)
            {
                return NotFound("The requested user was not found.");
            }

            return PartialView("_Edit", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserDTO user)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Edit", user);
            }

            var saved = await _userService.SaveAsync(user);

            if (!saved)
            {
                ViewData["ModalError"] = "The user could not be updated.";
                return PartialView("_Edit", user);
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user is null)
            {
                return NotFound("The requested user was not found.");
            }

            return PartialView("_Delete", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(UserDTO user)
        {
            var deleted = await _userService.DeleteAsync(user.UserId);

            if (!deleted)
            {
                ViewData["ModalError"] = "The user could not be deleted.";
                return PartialView("_Delete", user);
            }

            return Json(new { success = true });
        }
    }
}
