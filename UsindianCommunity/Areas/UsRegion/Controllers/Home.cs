using Microsoft.AspNetCore.Mvc;

namespace UsindianCommunity.Areas.UsRegion.Controllers
{
    [Area("UsRegion")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Area = TempData["Area"] = "UsRegion";
            return View();
        }
    }
}
