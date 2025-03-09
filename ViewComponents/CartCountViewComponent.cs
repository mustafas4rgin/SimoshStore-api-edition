using System.Security.Claims;
using App.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Namespace;

namespace SimoshStore;

public class CartCountViewComponent : ViewComponent
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDataRepository _Repository;
    private readonly IUserService _userService;
    public CartCountViewComponent(IUserService userService, IHttpContextAccessor httpContextAccessor, IDataRepository Repository)
    {
        _userService = userService;
        _Repository = Repository;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = GetUserId();
        int count = 0;
        if (userId is null)
        {
            return View(0);
        }
        var cartItems = await _Repository.GetAll<CartItemEntity>().Where(c => c.UserId == 1).ToListAsync();
        foreach(var item in cartItems)
        {
            count += item.Quantity;
        }
        return View(count);
    }
     private int? GetUserId()
        {
            return int.TryParse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId) ? userId : null;
        }
}
