using System.Security.Claims;
using System.Security.Principal;
using App.Data.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SimoshStore;

namespace MyApp.Namespace
{
    public class AuthController(IHttpClientFactory httpClientFactory) : Controller
    {
        private HttpClient Client => httpClientFactory.CreateClient("Api.Data");

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var response = await Client.PostAsJsonAsync("/api/login", model);

            if (!response.IsSuccessStatusCode)
            {
                return View(model);
            }

            var user = await response.Content.ReadFromJsonAsync<UserEntity>();

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                return View(model);
            }
            await LoginAsync(user);

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> LoginAsync(UserEntity user)
        {
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.FirstName),
                new(ClaimTypes.Surname, user.LastName),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role.Name),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
            return RedirectToAction("Index", "Home");
        }
        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        //     [HttpPost("Register")]
        //     public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        //     {
        //         if (!ModelState.IsValid)
        //         {
        //             return View(model);
        //         }

        //         var registerDto = new RegisterDto
        //         {
        //             FirstName = model.FirstName,
        //             LastName = model.LastName,
        //             Email = model.Email,
        //             Password = model.Password,
        //             Phone = model.Phone
        //         };

        //         var result = await _authService.RegisterAsync(registerDto);

        //         if (!result.Success)
        //         {
        //             ViewBag.Error = result.Message;
        //             return View(model);
        //         }

        //         return RedirectToAction("Login");
        //     }
        //     public async Task<IActionResult> Logout()
        //     {
        //         var result = await _authService.LogOutAsync();
        //         if (!result.Success)
        //         {
        //             ViewBag.Error = result.Message;
        //         }
        //         return RedirectToAction("Index", "Home");
        //     }
        //     [HttpGet]
        //     public IActionResult ForgotPassword()
        //     {
        //         return View();
        //     }

        //     [HttpPost]
        //     public async Task<IActionResult> ForgotPassword(string email)
        //     {
        //         var result = await _authService.ForgotPasswordAsync(email);

        //         if (result.Success)
        //         {
        //             TempData["Message"] = result.Message;
        //             return RedirectToAction("ForgotPasswordConfirmation");
        //         }
        //         else
        //         {
        //             ModelState.AddModelError("", result.Message);
        //             return View();
        //         }
        //     }
        //     [HttpGet]
        //     public IActionResult ResetPassword(string token)
        //     {
        //         if (string.IsNullOrEmpty(token))
        //         {
        //             return RedirectToAction("NotFound","Error");
        //         }

        //         return View(new ResetPasswordViewModel { Token = token });
        //     }

        //     [HttpPost]
        //     public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        //     {
        //         if (ModelState.IsValid)
        //         {
        //             // Token'ı doğrula ve şifreyi güncelle
        //             var result = await _authService.ResetPasswordAsync(model.Token, model.NewPassword);

        //             if (result.Success)
        //             {
        //                 TempData["Message"] = "Şifreniz başarıyla sıfırlandı.";
        //                 return RedirectToAction("Login");
        //             }
        //             else
        //             {
        //                 ModelState.AddModelError("", result.Message);
        //                 return View();
        //             }
        //         }
        //         return View(model);
        //     }
        //     [HttpGet]
        //     public IActionResult ForgotPasswordConfirmation()
        //     {
        //         return View();
        //     }
    }
}
