////using System;
using LogicLevel.DefinationRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UsindianCommunity.Areas.IndiaRegion.Controllers
{
    [Area("indiaRegion")]
    public class AutoMobileController : Controller
    {
        private readonly IIndiaRepository indiaRepository;

        public AutoMobileController(IIndiaRepository indiaRepository)
        {

            this.indiaRepository = indiaRepository;
        }
        [HttpGet]
        public IActionResult AllTwoWheel()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public IActionResult SaleYourVehicle()
        {
            ViewBag.Area = "indiaRegion";
            TempData["Area"] = "indiaRegion";
            return View();
        }
        [HttpGet]
        public IActionResult CarInSale()
        {
            ViewBag.Area = "indiaRegion";
            TempData["Area"] = "indiaRegion";
            return View();
        }
    }
}
