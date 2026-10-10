using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRoleService roleService, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 25;
            var allRoles = (await _roleService.GetRolesAsync()).ToList();
            var totalPages = Math.Max(1, (int)Math.Ceiling(allRoles.Count / (double)pageSize));
            page = Math.Clamp(page, 1, totalPages);

            var roles = allRoles
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewData["Pagination"] = new PaginationViewModel
            {
                CurrentPage = page,
                TotalPages = totalPages,
                ControllerName = "Role"
            };

            return View(roles);
        }

        // Esta acción devuelve únicamente el contenido del modal, no una vista completa.
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);

            if (role is null)
            {
                return NotFound("The requested role was not found.");
            }

            return PartialView("_Details", role);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Create", new RoleDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleDTO role)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Create", role);
            }

            var saved = await _roleService.SaveAsync(role);

            if (!saved)
            {
                ViewData["ModalError"] = "The role could not be created.";
                return PartialView("_Create", role);
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);

            if (role is null)
            {
                return NotFound("The requested role was not found.");
            }

            return PartialView("_Edit", role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleDTO role)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Edit", role);
            }

            var saved = await _roleService.SaveAsync(role);

            if (!saved)
            {
                ViewData["ModalError"] = "The role could not be updated.";
                return PartialView("_Edit", role);
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);

            if (role is null)
            {
                return NotFound("The requested role was not found.");
            }

            return PartialView("_Delete", role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(RoleDTO role)
        {
            var deleted = await _roleService.DeleteAsync(role.RoleId);

            if (!deleted)
            {
                ViewData["ModalError"] = "The role could not be deleted.";
                return PartialView("_Delete", role);
            }

            return Json(new { success = true });
        }
    }
}