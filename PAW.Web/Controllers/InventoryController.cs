using Microsoft.AspNetCore.Mvc;
using PAW.Web.Services;
using System.Threading.Tasks;

namespace PAW.Web.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(IInventoryService inventoryService, ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _inventoryService.GetInventoriesAsync();
            return View(result);
        }
    }
}


