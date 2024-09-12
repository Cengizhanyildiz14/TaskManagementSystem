using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Threading.Tasks;
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
        [Authorize(Policy = ("IK"))]
        public async Task<IActionResult> Update(int id)
        {
            var response = await _announcementService.GetAnnouncementById<APIResponse>(id);
            if (response == null || !response.IsSuccess)
            {
                return NotFound();
            }

            var announcement = JsonConvert.DeserializeObject<AnnouncementUpdateDto>(Convert.ToString(response.result));
            if (announcement == null)
            {
                return NotFound("Duyuru yüklenemedi.");
            }

            return View(announcement);
        }

        [HttpPost]
        [Authorize(Policy = ("IK"))]
        public async Task<IActionResult> Update(AnnouncementUpdateDto announcementUpdateDto)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = HttpContext.User.FindFirst(c => c.Type == "FullName")?.Value;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
            {
                return Unauthorized("Kullanıcı bilgileri doğrulanamadı.");
            }

            announcementUpdateDto.AuthorId = int.Parse(userId);
            announcementUpdateDto.AuthorName = userName;
            announcementUpdateDto.UpdatedDate = DateTime.Now;

            var updateResponse = await _announcementService.UpdateAnnouncement<APIResponse>(announcementUpdateDto.Id, announcementUpdateDto);

            if (updateResponse == null || !updateResponse.IsSuccess)
            {
                return View(announcementUpdateDto);
            }

            return RedirectToAction("Home");
        }

        [HttpGet]
        [Authorize(Policy = ("IK"))]
        public async Task<IActionResult> Create()
        {
            var announcementCreateDto = new AnnouncementCreateDto();
            return View(announcementCreateDto);
        }

        [HttpPost]
        [Authorize(Policy = ("IK"))]
        public async Task<IActionResult> Create(AnnouncementCreateDto announcementCreateDto)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = HttpContext.User.FindFirst(c => c.Type == "FullName")?.Value;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
            {
                return Unauthorized("Kullanıcı bilgileri doğrulanamadı.");
            }

            announcementCreateDto.CreatedDate = DateTime.Now;
            announcementCreateDto.AuthorId = int.Parse(userId);
            announcementCreateDto.UpdatedDate = null;
            announcementCreateDto.AuthorName = userName;
            announcementCreateDto.IsActive = true;

            var createRespone = await _announcementService.CreateAnnouncement<APIResponse>(announcementCreateDto);
            if (createRespone == null || !createRespone.IsSuccess)
            {
                return View(announcementCreateDto);
            }

            return RedirectToAction("Home");
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
