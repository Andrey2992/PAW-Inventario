using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;
        private readonly IProductService _productService;
        private readonly IInventoryService _inventoryService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<SupplierController> _logger;

        public SupplierController(
            ISupplierService supplierService,
            IProductService productService,
            IInventoryService inventoryService,
            ICategoryService categoryService,
            ILogger<SupplierController> logger)
        {
            _supplierService = supplierService;
            _productService = productService;
            _inventoryService = inventoryService;
            _categoryService = categoryService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 25;
            var allSuppliers = (await _supplierService.GetSuppliersAsync())
                .OrderBy(supplier => supplier.SupplierId)
                .ToList();

            var totalPages = Math.Max(1, (int)Math.Ceiling(allSuppliers.Count / (double)pageSize));
            page = Math.Clamp(page, 1, totalPages);

            var suppliers = allSuppliers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewData["Pagination"] = new PaginationViewModel
            {
                CurrentPage = page,
                TotalPages = totalPages,
                ControllerName = "Supplier"
            };

            return View(suppliers);
        }

        // Devuelve el supplier junto con sus productos y, de cada producto, su inventario, categoría y supplier.
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);

            if (supplier is null)
            {
                return NotFound("The requested supplier was not found.");
            }

            var products = (await _productService.GetProductsAsync())
                .Where(product => product.SupplierId == id)
                .OrderBy(product => product.ProductId)
                .ToList();

            var inventories = await _inventoryService.GetInventoriesAsync();
            var categories = await _categoryService.GetCategoriesAsync();
            var suppliers = await _supplierService.GetSuppliersAsync();

            var details = new SupplierDetailsViewModel
            {
                Supplier = supplier,
                Products = ProductDetailsViewModel.BuildList(products, inventories, categories, suppliers)
            };

            return PartialView("_Details", details);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Create", new SupplierDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierDTO supplier)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Create", supplier);
            }

            supplier.SupplierId = 0;

            if (!await TrySaveAsync(supplier))
            {
                ViewData["ModalError"] = "The supplier could not be created.";
                return PartialView("_Create", supplier);
            }

            return Json(new { success = true });
        }

        public async Task<SupplierDTO?> GetSupplier(int id)
        {
            return await _supplierService.GetSupplierByIdAsync(id);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);

            if (supplier is null)
            {
                return NotFound("The requested supplier was not found.");
            }

            return PartialView("_Edit", supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SupplierDTO supplier)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Edit", supplier);
            }

            if (!await TrySaveAsync(supplier))
            {
                ViewData["ModalError"] = "The supplier could not be updated.";
                return PartialView("_Edit", supplier);
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);

            if (supplier is null)
            {
                return NotFound("The requested supplier was not found.");
            }

            // Un supplier con productos no se puede borrar (llave foránea), se avisa antes de intentarlo.
            ViewData["ProductCount"] = (await _productService.GetProductsAsync())
                .Count(product => product.SupplierId == id);

            return PartialView("_Delete", supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(SupplierDTO supplier)
        {
            bool deleted;

            try
            {
                deleted = await _supplierService.DeleteAsync(supplier.SupplierId);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error deleting supplier {SupplierId}", supplier.SupplierId);
                deleted = false;
            }

            if (!deleted)
            {
                ViewData["ModalError"] = "The supplier could not be deleted. It may still have products assigned.";
                return PartialView("_Delete", supplier);
            }

            return Json(new { success = true });
        }

        private async Task<bool> TrySaveAsync(SupplierDTO supplier)
        {
            try
            {
                return await _supplierService.SaveAsync(supplier);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error saving supplier {SupplierId}", supplier.SupplierId);
                return false;
            }
        }
    }
}