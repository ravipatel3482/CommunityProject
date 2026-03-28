using Microsoft.AspNetCore.Mvc;
namespace UsindianCommunity.Areas.IndiaRegion.Controllers
{
    [Area("indiaRegion")]
    public class BusinessController : Controller
    {
        public BusinessController()
        {

        }
        [HttpGet]
        public IActionResult BusinessInsale()
        {
            TempData["Area"] = "indiaRegion";
            return View();
        }
        [HttpGet]
        public IActionResult SaleYourBusiness()
        {
            TempData["Area"] = "indiaRegion";
            return View();
        }
    }
}
