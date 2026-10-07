using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IInventoryService _inventoryService;
        private readonly ICategoryService _categoryService;
        private readonly ISupplierService _supplierService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(
            IProductService productService,
            IInventoryService inventoryService,
            ICategoryService categoryService,
            ISupplierService supplierService,
            ILogger<ProductController> logger)
        {
            _productService = productService;
            _inventoryService = inventoryService;
            _categoryService = categoryService;
            _supplierService = supplierService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 25;
            var allProducts = (await _productService.GetProductsAsync())
                .OrderBy(product => product.ProductId)
                .ToList();

            var totalPages = Math.Max(1, (int)Math.Ceiling(allProducts.Count / (double)pageSize));
            page = Math.Clamp(page, 1, totalPages);

            var products = allProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewData["Pagination"] = new PaginationViewModel
            {
                CurrentPage = page,
                TotalPages = totalPages,
                ControllerName = "Product"
            };

            return View(products);
        }

        // Devuelve el detalle del producto junto con su inventario, categoría y supplier asociados.
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product is null)
            {
                return NotFound("The requested product was not found.");
            }

            var inventories = await _inventoryService.GetInventoriesAsync();
            var categories = await _categoryService.GetCategoriesAsync();
            var suppliers = await _supplierService.GetSuppliersAsync();

            var details = new ProductDetailsViewModel
            {
                Product = product,
                // El producto apunta a su inventario; si no, se busca el inventario que apunta al producto.
                Inventory = inventories.FirstOrDefault(i => i.InventoryId == product.InventoryId)
                    ?? inventories.FirstOrDefault(i => i.ProductId == product.ProductId),
                Category = categories.FirstOrDefault(c => c.CategoryId == product.CategoryId),
                Supplier = suppliers.FirstOrDefault(s => s.SupplierId == product.SupplierId)
            };

            return PartialView("_Details", details);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDropdownsAsync();
            return PartialView("_Create", new ProductDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDTO product)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return PartialView("_Create", product);
            }

            product.ProductId = 0;
            product.ModifiedBy = product.CreatedBy;

            if (!await TrySaveAsync(product))
            {
                ViewData["ModalError"] = "The product could not be created.";
                await LoadDropdownsAsync();
                return PartialView("_Create", product);
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product is null)
            {
                return NotFound("The requested product was not found.");
            }

            await LoadDropdownsAsync();
            return PartialView("_Edit", product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductDTO product)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return PartialView("_Edit", product);
            }

            if (!await TrySaveAsync(product))
            {
                ViewData["ModalError"] = "The product could not be updated.";
                await LoadDropdownsAsync();
                return PartialView("_Edit", product);
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product is null)
            {
                return NotFound("The requested product was not found.");
            }

            return PartialView("_Delete", product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ProductDTO product)
        {
            bool deleted;

            try
            {
                deleted = await _productService.DeleteAsync(product.ProductId);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error deleting product {ProductId}", product.ProductId);
                deleted = false;
            }

            if (!deleted)
            {
                ViewData["ModalError"] = "The product could not be deleted.";
                return PartialView("_Delete", product);
            }

            return Json(new { success = true });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<bool> TrySaveAsync(ProductDTO product)
        {
            try
            {
                return await _productService.SaveAsync(product);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error saving product {ProductId}", product.ProductId);
                return false;
            }
        }

        private async Task LoadDropdownsAsync()
        {
            ViewBag.Categories = (await _categoryService.GetCategoriesAsync())
                .OrderBy(c => c.CategoryName)
                .ToList();
            ViewBag.Suppliers = (await _supplierService.GetSuppliersAsync())
                .OrderBy(s => s.SupplierName)
                .ToList();
            ViewBag.Inventories = (await _inventoryService.GetInventoriesAsync())
                .OrderBy(i => i.InventoryId)
                .ToList();
        }
    }
}
