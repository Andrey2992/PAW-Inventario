using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class ComponentController : Controller
    {
        private readonly IComponentService _componentService;
        private readonly ILogger<ComponentController> _logger;

        public ComponentController(IComponentService componentService, ILogger<ComponentController> logger)
        {
            _componentService = componentService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 25;
            var allComponents = (await _componentService.GetComponentsAsync())
                .OrderBy(component => component.Id)
                .ToList();

            var totalPages = Math.Max(1, (int)Math.Ceiling(allComponents.Count / (double)pageSize));
            page = Math.Clamp(page, 1, totalPages);

            var components = allComponents
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewData["Pagination"] = new PaginationViewModel
            {
                CurrentPage = page,
                TotalPages = totalPages,
                ControllerName = "Component"
            };

            return View(components);
        }

        // Component no tiene relación con inventario, categoría ni supplier, así que solo muestra sus propios datos.
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var component = await _componentService.GetComponentByIdAsync(id);

            if (component is null)
            {
                return NotFound("The requested component was not found.");
            }

            return PartialView("_Details", component);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Create", new ComponentDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ComponentDTO component)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Create", component);
            }

            component.Id = 0;

            if (!await TrySaveAsync(component))
            {
                ViewData["ModalError"] = "The component could not be created.";
                return PartialView("_Create", component);
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var component = await _componentService.GetComponentByIdAsync(id);

            if (component is null)
            {
                return NotFound("The requested component was not found.");
            }

            return PartialView("_Edit", component);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ComponentDTO component)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Edit", component);
            }

            if (!await TrySaveAsync(component))
            {
                ViewData["ModalError"] = "The component could not be updated.";
                return PartialView("_Edit", component);
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var component = await _componentService.GetComponentByIdAsync(id);

            if (component is null)
            {
                return NotFound("The requested component was not found.");
            }

            return PartialView("_Delete", component);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ComponentDTO component)
        {
            bool deleted;

            try
            {
                deleted = await _componentService.DeleteAsync((int)component.Id);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error deleting component {ComponentId}", component.Id);
                deleted = false;
            }

            if (!deleted)
            {
                ViewData["ModalError"] = "The component could not be deleted.";
                return PartialView("_Delete", component);
            }

            return Json(new { success = true });
        }

        private async Task<bool> TrySaveAsync(ComponentDTO component)
        {
            try
            {
                return await _componentService.SaveAsync(component);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error saving component {ComponentId}", component.Id);
                return false;
            }
        }
    }
}

