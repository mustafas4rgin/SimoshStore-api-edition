using System.Security.Claims;
using App.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SimoshStore;

public class AccountViewComponent : ViewComponent
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProfileService _profileService;
    public AccountViewComponent(IHttpContextAccessor httpContextAccessor, IProfileService profileService)
    {
        _profileService = profileService;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userIdClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
        if (userIdClaim == null)
        {
            ViewData["AuthError"] = "You need to login to view this page";
            return View(new UserEntity());
        }
        int userId = int.Parse(userIdClaim);
        var user = await _profileService.GetUserByIdAsync(userId);
        return View(user);
    }
}
