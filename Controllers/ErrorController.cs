using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    public class ErrorController : Controller
    {
        public IActionResult NotFound()
        {
            return View();
        }
        public IActionResult UnderConstruction()
        {
            return View();
        }
    }
}
