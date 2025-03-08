using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SimoshStore;

namespace MyApp.Namespace
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthService authService, IAuthRepository authRepository)
        {
            _authService = authService;
            _authRepository = authRepository;
        }
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(LoginViewModel model, string? ReturnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            var loginDto = new LoginDto(model.Email, model.Password, model.RememberMe);
            var result = await _authService.LoginAsync(loginDto);

            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View(model);
            }

            if (!string.IsNullOrEmpty(ReturnUrl))
                return Redirect(ReturnUrl);

            return RedirectToAction("Index", "Home");
        }
        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var registerDto = new RegisterDto
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password,
                Phone = model.Phone
            };

            var result = await _authService.RegisterAsync(registerDto);

            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View(model);
            }

            return RedirectToAction("Login");
        }
        public async Task<IActionResult> Logout()
        {
            var result = await _authService.LogOutAsync();
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var result = await _authService.ForgotPasswordAsync(email);

            if (result.Success)
            {
                TempData["Message"] = result.Message;
                return RedirectToAction("ForgotPasswordConfirmation");
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View();
            }
        }
        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("NotFound","Error");
            }

            return View(new ResetPasswordViewModel { Token = token });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Token'ı doğrula ve şifreyi güncelle
                var result = await _authService.ResetPasswordAsync(model.Token, model.NewPassword);

                if (result.Success)
                {
                    TempData["Message"] = "Şifreniz başarıyla sıfırlandı.";
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                    return View();
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
    }
}
