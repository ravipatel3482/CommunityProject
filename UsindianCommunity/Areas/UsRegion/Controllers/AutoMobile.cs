using Microsoft.AspNetCore.Mvc;

namespace UsindianCommunity.Areas.UsRegion.Controllers
{
    [Area("UsRegion")]
    public class AutoMobile : Controller
    {
        [HttpGet]
        public IActionResult CarInSale()
        {
            ViewBag.Area = TempData["Area"] = "UsRegion";
            return View();
        }
        [HttpGet]
        public IActionResult SaleYourVehicle()
        {
            ViewBag.Area = TempData["Area"] = "UsRegion";
            return View();
        }
    }
}
