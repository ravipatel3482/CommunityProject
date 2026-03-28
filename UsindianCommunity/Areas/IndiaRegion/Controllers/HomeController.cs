using Microsoft.AspNetCore.Mvc;

namespace UsindianCommunity.Areas.IndiaRegion.Controllers
{
    [Area("indiaRegion")]
    public class HomeController : Controller
    {

        public HomeController()
        {

        }
        [HttpGet]
        public IActionResult Index()
        {
            TempData["Area"] = "indiaRegion";
            return View();
        }

    }
}
