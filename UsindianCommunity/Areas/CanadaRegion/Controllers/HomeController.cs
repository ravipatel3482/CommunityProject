using Microsoft.AspNetCore.Mvc;

namespace UsindianCommunity.Areas.CanadaRegion.Controllers
{
    [Area("CanadaRegion")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
