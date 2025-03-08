using System.Security.Claims;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace SimoshStore;

public interface IAuthService
{
    Task<IServiceResult> LoginAsync(LoginDto login);
    Task<IServiceResult> LogOutAsync();
    Task<IServiceResult> RegisterAsync(RegisterDto register);
    Task<IServiceResult> ForgotPasswordAsync(string email);
    Task<IServiceResult> ResetPasswordAsync(string token, string newPassword);
}
