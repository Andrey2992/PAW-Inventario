using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class UserRoleController : Controller
    {
        private readonly IUserRoleService _userRoleService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ILogger<UserRoleController> _logger;
        public UserRoleController(
            IUserRoleService userRoleService,
            IUserService userService,
            IRoleService roleService,
            ILogger<UserRoleController> logger)
        {
            _userRoleService = userRoleService;
            _userService = userService;
            _roleService = roleService;
            _logger = logger;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 25;
            var allItems = (await _userRoleService.GetUserRolesAsync())
                .OrderBy(item => item.Id)
                .ToList();

            var totalPages = Math.Max(1,
                (int)Math.Ceiling(allItems.Count / (double)pageSize));
            page = Math.Clamp(page, 1, totalPages);

            var items = allItems
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewData["Pagination"] = new PaginationViewModel
            {
                CurrentPage = page,
                TotalPages = totalPages,
                ControllerName = "UserRole"
            };

            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userRole = await _userRoleService.GetUserRoleByIdAsync(id);

            if (userRole is null)
            {
                return NotFound("The requested user-role assignment was not found.");
            }

            return PartialView("_Details", userRole);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDropdownsAsync();
            return PartialView("_Create", new UserRoleDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserRoleDTO userRole)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return PartialView("_Create", userRole);
            }

            var saved = await _userRoleService.SaveAsync(userRole);

            if (!saved)
            {
                ViewData["ModalError"] = "The user-role assignment could not be created.";
                await LoadDropdownsAsync();
                return PartialView("_Create", userRole);
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userRole = await _userRoleService.GetUserRoleByIdAsync(id);

            if (userRole is null)
            {
                return NotFound("The requested user-role assignment was not found.");
            }

            await LoadDropdownsAsync();
            return PartialView("_Edit", userRole);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserRoleDTO userRole)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return PartialView("_Edit", userRole);
            }

            var saved = await _userRoleService.SaveAsync(userRole);

            if (!saved)
            {
                ViewData["ModalError"] = "The user-role assignment could not be updated.";
                await LoadDropdownsAsync();
                return PartialView("_Edit", userRole);
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userRole = await _userRoleService.GetUserRoleByIdAsync(id);

            if (userRole is null)
            {
                return NotFound("The requested user-role assignment was not found.");
            }

            return PartialView("_Delete", userRole);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(UserRoleDTO userRole)
        {
            var deleted = await _userRoleService.DeleteAsync(userRole.Id);

            if (!deleted)
            {
                ViewData["ModalError"] = "The user-role assignment could not be deleted.";
                return PartialView("_Delete", userRole);
            }

            return Json(new { success = true });
        }

        private async Task LoadDropdownsAsync()
        {
            ViewBag.Users = await _userService.GetUsersAsync();
            ViewBag.Roles = await _roleService.GetRolesAsync();
        }
    }
}
