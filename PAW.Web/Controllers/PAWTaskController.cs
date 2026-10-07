using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class PawTaskController : Controller
    {
        private readonly IPawTaskService _pawTaskService;
        private readonly ILogger<PawTaskController> _logger;

        public PawTaskController(IPawTaskService pawTaskService, ILogger<PawTaskController> logger)
        {
            _pawTaskService = pawTaskService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 25;
            var all = (await _pawTaskService.GetPawTasksAsync()).OrderBy(x => x.TaskId).ToList();
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
                ControllerName = "PawTask"
            };

            return View(items);
        }

        // Estas acciones devuelven únicamente el contenido del modal, no una vista completa.
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var pawTask = await _pawTaskService.GetPawTaskByIdAsync(id);

            if (pawTask is null)
            {
                return NotFound("The requested task was not found.");
            }

            return PartialView("_Details", pawTask);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Create", new PawTaskDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PawTaskDTO pawTask)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Create", pawTask);
            }

            var saved = await _pawTaskService.SaveAsync(pawTask);

            if (!saved)
            {
                ViewData["ModalError"] = "The task could not be created.";
                return PartialView("_Create", pawTask);
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var pawTask = await _pawTaskService.GetPawTaskByIdAsync(id);

            if (pawTask is null)
            {
                return NotFound("The requested task was not found.");
            }

            return PartialView("_Edit", pawTask);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PawTaskDTO pawTask)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Edit", pawTask);
            }

            var saved = await _pawTaskService.SaveAsync(pawTask);

            if (!saved)
            {
                ViewData["ModalError"] = "The task could not be updated.";
                return PartialView("_Edit", pawTask);
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var pawTask = await _pawTaskService.GetPawTaskByIdAsync(id);

            if (pawTask is null)
            {
                return NotFound("The requested task was not found.");
            }

            return PartialView("_Delete", pawTask);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(PawTaskDTO pawTask)
        {
            var deleted = await _pawTaskService.DeleteAsync(pawTask.TaskId);

            if (!deleted)
            {
                ViewData["ModalError"] = "The task could not be deleted.";
                return PartialView("_Delete", pawTask);
            }

            return Json(new { success = true });
        }
    }
}
