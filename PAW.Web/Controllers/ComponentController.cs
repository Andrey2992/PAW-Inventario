using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Index()
        {
            var result = await _componentService.GetComponentsAsync();
            return View(result);
        }
    }
}
