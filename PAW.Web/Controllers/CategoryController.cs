using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IInventoryService _inventoryService;
        private readonly ISupplierService _supplierService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(
            ICategoryService categoryService,
            IProductService productService,
            IInventoryService inventoryService,
            ISupplierService supplierService,
            ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _productService = productService;
            _inventoryService = inventoryService;
            _supplierService = supplierService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 25;
            var allCategories = (await _categoryService.GetCategoriesAsync())
                .OrderBy(category => category.CategoryId)
                .ToList();

            var totalPages = Math.Max(1, (int)Math.Ceiling(allCategories.Count / (double)pageSize));
            page = Math.Clamp(page, 1, totalPages);

            var categories = allCategories
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewData["Pagination"] = new PaginationViewModel
            {
                CurrentPage = page,
                TotalPages = totalPages,
                ControllerName = "Category"
            };

            return View(categories);
        }

        // Devuelve la categoría junto con sus productos y, de cada producto, su inventario, categoría y supplier.
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category is null)
            {
                return NotFound("The requested category was not found.");
            }

            var products = (await _productService.GetProductsAsync())
                .Where(product => product.CategoryId == id)
                .OrderBy(product => product.ProductId)
                .ToList();

            var inventories = await _inventoryService.GetInventoriesAsync();
            var categories = await _categoryService.GetCategoriesAsync();
            var suppliers = await _supplierService.GetSuppliersAsync();

            var details = new CategoryDetailsViewModel
            {
                Category = category,
                Products = ProductDetailsViewModel.BuildList(products, inventories, categories, suppliers)
            };

            return PartialView("_Details", details);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Create", new CategoryDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryDTO category)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Create", category);
            }

            category.CategoryId = 0;

            if (!await TrySaveAsync(category))
            {
                ViewData["ModalError"] = "The category could not be created.";
                return PartialView("_Create", category);
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category is null)
            {
                return NotFound("The requested category was not found.");
            }

            return PartialView("_Edit", category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryDTO category)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Edit", category);
            }

            if (!await TrySaveAsync(category))
            {
                ViewData["ModalError"] = "The category could not be updated.";
                return PartialView("_Edit", category);
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category is null)
            {
                return NotFound("The requested category was not found.");
            }

            // Una categoría con productos no se puede borrar (llave foránea), se avisa antes de intentarlo.
            ViewData["ProductCount"] = (await _productService.GetProductsAsync())
                .Count(product => product.CategoryId == id);

            return PartialView("_Delete", category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(CategoryDTO category)
        {
            bool deleted;

            try
            {
                deleted = await _categoryService.DeleteAsync(category.CategoryId);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error deleting category {CategoryId}", category.CategoryId);
                deleted = false;
            }

            if (!deleted)
            {
                ViewData["ModalError"] = "The category could not be deleted. It may still have products assigned.";
                return PartialView("_Delete", category);
            }

            return Json(new { success = true });
        }

        private async Task<bool> TrySaveAsync(CategoryDTO category)
        {
            try
            {
                return await _categoryService.SaveAsync(category);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error saving category {CategoryId}", category.CategoryId);
                return false;
            }
        }
    }
}


