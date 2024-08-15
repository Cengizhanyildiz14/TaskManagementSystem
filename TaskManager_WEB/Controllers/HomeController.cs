using Microsoft.AspNetCore.Mvc;

namespace TaskManager_WEB.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet]
        public async Task<IActionResult> NotFoundPage()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
    }
}
