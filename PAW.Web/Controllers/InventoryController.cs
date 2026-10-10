using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;
using System.Globalization;
using System.Threading.Tasks;

namespace PAW.Web.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryService _inventoryService;
        private readonly IProductService _productService;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(
            IInventoryService inventoryService,
            IProductService productService,
            ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _productService = productService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 25;
            var allItems = (await _inventoryService.GetInventoriesAsync())
                .OrderBy(item => item.InventoryId)
                .ToList();

            var totalPages = Math.Max(1, (int)Math.Ceiling(allItems.Count / (double)pageSize));
            page = Math.Clamp(page, 1, totalPages);

            var items = allItems
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewData["Pagination"] = new PaginationViewModel
            {
                CurrentPage = page,
                TotalPages = totalPages,
                ControllerName = "Inventory"
            };

            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var inventory = await _inventoryService.GetInventoryByIdAsync(id);

            if (inventory is null)
            {
                return NotFound("The requested inventory record was not found.");
            }

            if (inventory.ProductId is int productId)
            {
                ViewData["ProductName"] = (await _productService.GetProductByIdAsync(productId))?.Name;
            }

            return PartialView("_Details", inventory);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDropdownsAsync();
            return PartialView("_Create", new InventoryDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InventoryDTO inventory)
        {
            BindUnitPrice(inventory);

            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return PartialView("_Create", inventory);
            }

            inventory.InventoryId = 0;
            inventory.DateAdded = DateTime.Now;
            inventory.LastUpdated = DateTime.Now;

            if (!await TrySaveAsync(inventory))
            {
                ViewData["ModalError"] = "The inventory record could not be created.";
                await LoadDropdownsAsync();
                return PartialView("_Create", inventory);
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var inventory = await _inventoryService.GetInventoryByIdAsync(id);

            if (inventory is null)
            {
                return NotFound("The requested inventory record was not found.");
            }

            await LoadDropdownsAsync();
            return PartialView("_Edit", inventory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(InventoryDTO inventory)
        {
            BindUnitPrice(inventory);

            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return PartialView("_Edit", inventory);
            }

            inventory.LastUpdated = DateTime.Now;

            if (!await TrySaveAsync(inventory))
            {
                ViewData["ModalError"] = "The inventory record could not be updated.";
                await LoadDropdownsAsync();
                return PartialView("_Edit", inventory);
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var inventory = await _inventoryService.GetInventoryByIdAsync(id);

            if (inventory is null)
            {
                return NotFound("The requested inventory record was not found.");
            }

            return PartialView("_Delete", inventory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(InventoryDTO inventory)
        {
            bool deleted;

            try
            {
                deleted = await _inventoryService.DeleteAsync(inventory.InventoryId);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error deleting inventory {InventoryId}", inventory.InventoryId);
                deleted = false;
            }

            if (!deleted)
            {
                // Lo más común es que un producto todavía esté usando este inventario.
                ViewData["ModalError"] = "The inventory record could not be deleted. It may still be assigned to a product.";
                return PartialView("_Delete", inventory);
            }

            return Json(new { success = true });
        }

        // El input type="number" siempre envía el decimal con punto, sin importar la cultura del servidor.
        private void BindUnitPrice(InventoryDTO inventory)
        {
            var rawValue = Request.Form[nameof(InventoryDTO.UnitPrice)].ToString();

            if (!decimal.TryParse(rawValue, NumberStyles.Number, CultureInfo.InvariantCulture, out var unitPrice))
            {
                return;
            }

            inventory.UnitPrice = unitPrice;
            ModelState.ClearValidationState(string.Empty);
            TryValidateModel(inventory);
        }

        private async Task<bool> TrySaveAsync(InventoryDTO inventory)
        {
            try
            {
                return await _inventoryService.SaveAsync(inventory);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error saving inventory {InventoryId}", inventory.InventoryId);
                return false;
            }
        }

        private async Task LoadDropdownsAsync()
        {
            ViewBag.Products = (await _productService.GetProductsAsync())
                .OrderBy(p => p.Name)
                .ToList();
        }
    }
}
