using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace UsindianCommunity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            TempData["Area"] = string.Empty;
            return View();
        }
    }
}
