using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace UsindianCommunity.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> logger;
        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }
        [AllowAnonymous]
        [Route("Error")]
        public IActionResult Error()
        {

            ViewBag.Area = TempData["Area"];
            var execeptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            logger.LogError($"The Path {execeptionHandlerPathFeature.Path} " + $"threw an Exception {execeptionHandlerPathFeature.Error}");
            return View("Error");
        }
        [Route("Error/{statusCode}")]
        public IActionResult ErrorHandler(int statusCode)
        {
            ViewBag.Area = TempData["Area"];

            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the resource you requested could not be found";
                    break;
            }

            return View();
        }

    }
}
