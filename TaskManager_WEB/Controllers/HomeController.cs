using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TaskManager_WEB.Models;
using TaskManager_WEB.Services.IServices;

namespace TaskManager_WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnnouncementService _announcementService;
        private readonly IMapper _mapper;

        public HomeController(IAnnouncementService announcementService, IMapper mapper)
        {
            _announcementService = announcementService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Home()
        {
            var announcementResponse = await _announcementService.GetAll<APIResponse>();

            if (announcementResponse == null || !announcementResponse.IsSuccess)
            {
                return NotFound();
            }

            var announcementList = JsonConvert.DeserializeObject<List<AnnouncementDto>>(Convert.ToString(announcementResponse.result));

            return View(announcementList);
        }

        [HttpPost]
        [Authorize(Policy = ("IK"))]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _announcementService.Delete<APIResponse>(id);
            if (response == null || !response.IsSuccess)
            {
                return NotFound();
            }
            return RedirectToAction("home", "home");
        }

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

        public IActionResult ChangeLang(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)), new CookieOptions()
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1)
            });
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
