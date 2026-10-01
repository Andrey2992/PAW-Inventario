using Microsoft.AspNetCore.Mvc;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _categoryService.GetCategoriesAsync();
            return View(result);
        }
    }
}
