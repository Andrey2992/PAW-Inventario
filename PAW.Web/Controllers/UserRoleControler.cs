using Microsoft.AspNetCore.Mvc;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class UserRoleController : Controller
    {
        private readonly IUserRoleService _userRoleService;
        private readonly ILogger<UserRoleController> _logger;
        public UserRoleController(IUserRoleService userRoleService, ILogger<UserRoleController> logger)
        {
            _userRoleService = userRoleService;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var items = await _userRoleService.GetUserRolesAsync();
            return View(items);
        }
    }
}
