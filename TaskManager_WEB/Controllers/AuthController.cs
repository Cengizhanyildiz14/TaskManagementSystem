using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskManager_WEB.Models;
using TaskManager_WEB.Services.IServices;
using Utility;

namespace TaskManager_WEB.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("getallusers", "user");
            }
            LoginRequestDto dto = new();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto, bool rememberMe)
        {
            var response = await _authService.Login<APIResponse>(loginRequestDto);
            if (response != null && response.IsSuccess)
            {
                LoginResponseDto model = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.result));

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(model.Token);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, model.User.Id.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Email, model.User.Email));

                var departmentName = jwtToken.Claims.FirstOrDefault(c => c.Type == "DepartmentName")?.Value;
                if (!string.IsNullOrEmpty(departmentName))
                {
                    identity.AddClaim(new Claim("DepartmentName", departmentName));
                }

                var fullName = jwtToken.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value;
                if (!string.IsNullOrEmpty(fullName))
                {
                    identity.AddClaim(new Claim("FullName", fullName));
                }

                var gender = jwtToken.Claims.FirstOrDefault(c => c.Type == "Gender")?.Value;
                if (!string.IsNullOrEmpty(gender))
                {
                    identity.AddClaim(new Claim("Gender", gender));
                }

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = rememberMe ? DateTimeOffset.UtcNow.AddDays(1) : DateTimeOffset.UtcNow.AddMinutes(15),
                    AllowRefresh = true,
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties);

                return RedirectToAction("home", "home");
            }
            else
            {
                ModelState.AddModelError("CustomError", "Giriş başarısız.");
                return View(loginRequestDto);
            }
        }



        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("login", "auth");
        }
    }
}
