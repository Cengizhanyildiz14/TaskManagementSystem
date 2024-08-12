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
                // LoginResponseDto modelini API'den dönen sonucu deserialize ederek oluşturuyoruz
                LoginResponseDto model = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.result));

                // Token'ı çözümleyerek içerisindeki claim'leri alıyoruz
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(model.Token);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, model.User.Id.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Email, model.User.Email));

                // Token'dan DepartmentName claim'ini alıyoruz ve kimlik doğrulamaya ekliyoruz
                var departmentName = jwtToken.Claims.FirstOrDefault(c => c.Type == "DepartmentName")?.Value;
                if (!string.IsNullOrEmpty(departmentName))
                {
                    identity.AddClaim(new Claim("DepartmentName", departmentName));
                }

                // FullName claim'ini de ekleyelim
                var fullName = jwtToken.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value;
                if (!string.IsNullOrEmpty(fullName))
                {
                    identity.AddClaim(new Claim("FullName", fullName));
                }

                // Claim'lerin doğru eklenip eklenmediğini kontrol edelim
                var claims = identity.Claims.ToList();
                if (!claims.Any())
                {
                    throw new Exception("No claims were added to the identity. Check the login process.");
                }

                var principal = new ClaimsPrincipal(identity);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = rememberMe,
                    ExpiresUtc = rememberMe ? DateTimeOffset.UtcNow.AddDays(1) : DateTimeOffset.UtcNow.AddMinutes(15)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                // Token'ı hem session'da hem de cookie'de saklama
                HttpContext.Session.SetString(SD.SessionToken, model.Token);
                HttpContext.Response.Cookies.Append("AuthToken", model.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = authProperties.ExpiresUtc
                });

                // Oturum açma sonrası userId kontrolü
                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    throw new NullReferenceException("User ID is not found after login. Ensure that claims are correctly added.");
                }

                return RedirectToAction("getallusers", "Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", "Giriş başarısız.");
                return View(loginRequestDto);
            }
        }




        public async Task<IActionResult> LogOut()
        {
            HttpContext.Session.Clear();
            if (Request.Cookies.ContainsKey(SD.SessionToken))
            {
                Response.Cookies.Delete(SD.SessionToken);
            }

            // Kimlik doğrulamasını temizle
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);


            return RedirectToAction("getallusers", "Home");
        }

        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
    }
}
