using Microsoft.AspNetCore.Mvc;

namespace TaskManager_WEB.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet]
        public IActionResult NotFoundPage()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
