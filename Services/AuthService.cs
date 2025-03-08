using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using App.Data.Entities;

namespace SimoshStore
{
    public class AuthService : IAuthService
    {
        private readonly IDataRepository _dataRepository;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IAuthRepository _authRepository;
        private readonly ITokenService _tokenService;
        public AuthService(ITokenService tokenService, IDataRepository dataRepository, AppDbContext context, IHttpContextAccessor httpContextAccessor, IEmailService emailService, IAuthRepository authRepository)
        {
            _dataRepository = dataRepository;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _authRepository = authRepository;
            _tokenService = tokenService;
        }
        public async Task<IServiceResult> LoginAsync(LoginDto login)
        {
            var user = _authRepository.ValidateUser(login.Email, login.Password);


            if (user is null)
                return new ServiceResult(false, "Kullanıcı bulunamadı.");

            var role = _authRepository.GetRoles().Where(r => r.Id == user.RoleId).FirstOrDefault();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.MobilePhone, user.Phone),
                new Claim(ClaimTypes.Role, role.Name)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties
            {
                IsPersistent = login.RememberMe,
            };

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);

            return new ServiceResult(true, "Giriş başarılı");
        }
        public async Task<IServiceResult> RegisterAsync(RegisterDto register)
        {
            
            var existingUser = _authRepository.GetUsers().FirstOrDefault(u => u.Email == register.Email);
            if (existingUser != null)
            {
                return new ServiceResult(false, "Bu e-posta adresiyle zaten bir kullanıcı bulunuyor.");
            }

            HashingHelper.CreatePasswordHash(register.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new UserEntity
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                Email = register.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Phone = register.Phone,
                RoleId = 3,
                Enabled = true,
                CreatedAt = DateTime.UtcNow,
                ResetToken = string.Empty,
                ResetTokenExpires = DateTime.UtcNow,


            };            await _dataRepository.AddAsync(user);

            return new ServiceResult(true, "Kayıt başarılı.");
        }

        public async Task<IServiceResult> LogOutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return new ServiceResult(true, "Başarıyla çıkış yapıldı.");
        }
        public async Task<IServiceResult> ForgotPasswordAsync(string email)
        {
            var user = _authRepository.GetUsers().FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return new ServiceResult(false, "Bu e-posta adresiyle kayıtlı bir kullanıcı bulunamadı.");
            }

           
            var resetToken = _tokenService.GenerateToken(); 

            
            user.ResetToken = resetToken;
            user.ResetTokenExpires = DateTime.UtcNow.AddHours(1); 
            await _dataRepository.UpdateAsync(user);

            
            var resetLink = $"http://localhost:5095/Auth/ResetPassword?token={resetToken}";

            
            await _emailService.SendEmailAsync(user.Email, "Şifre Sıfırlama", $"Şifrenizi sıfırlamak için şu bağlantıyı tıklayın: {resetLink}");

            return new ServiceResult(true, "Şifre sıfırlama bağlantısı e-posta adresinize gönderildi.");
        }
        
        public async Task<IServiceResult> ResetPasswordAsync(string token, string newPassword)
        {
            var user = _authRepository.GetUsers().FirstOrDefault(u => u.ResetToken == token && u.ResetTokenExpires > DateTime.UtcNow);

            if (user == null)
            {
                return new ServiceResult(false, "Geçersiz veya süresi dolmuş şifre sıfırlama bağlantısı.");
            }

            HashingHelper.CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.ResetToken = string.Empty;
            user.ResetTokenExpires = DateTime.UtcNow;

            await _dataRepository.UpdateAsync(user);

            return new ServiceResult(true, "Şifreniz başarıyla sıfırlandı.");
        }

    }
}
