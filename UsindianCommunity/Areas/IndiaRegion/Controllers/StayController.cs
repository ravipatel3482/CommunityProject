using Microsoft.AspNetCore.Mvc;

namespace UsindianCommunity.Areas.IndiaRegion.Controllers
{
    [Area("indiaRegion")]
    public class StayController : Controller
    {
        public StayController()
        {
            ViewBag.Area = "indiaRegion";
        }
        [HttpGet]
        public IActionResult AvailableRoom()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Motel()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AddavilableRoom()
        {
            return View();
        }
        [HttpGet]
        public IActionResult PostRequirement()
        {
            return View();

        }
    }
}
