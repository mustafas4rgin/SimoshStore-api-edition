using System.Security.Claims;
using App.Data.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace SimoshStore;

public class CartViewComponent : ViewComponent
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDataRepository _Repository;
    public CartViewComponent(IHttpContextAccessor httpContextAccessor, IDataRepository Repository)
    {
        _Repository = Repository;
        _httpContextAccessor = httpContextAccessor;
    }
    public IViewComponentResult Invoke()
    {
        var userId = GetUserId();
        var products = _Repository.GetAll<ProductEntity>().ToList();
        var discounts = _Repository.GetAll<DiscountEntity>().ToList();
        var images = _Repository.GetAll<ProductImageEntity>().ToList();
        var nullCartItems = new List<CartItemEntity>();
        var nullProducts = new List<ProductEntity>();
        var nullImages = new List<ProductImageEntity>();
        var nullDiscounts = new List<DiscountEntity>();
        var cartItems = _Repository.GetAll<CartItemEntity>().Where(x => x.UserId == userId).ToList();
        int cartCount = cartItems.Count();
        ViewData["CartItemCount"] = cartCount;
        if (userId == null)
        {
            return View(new ShoppingCartViewModel
            {
                images = new List<ProductImageEntity>(),
                discounts = new List<DiscountEntity>(),
                cartItems = new List<CartItemEntity>(),
                products = new List<ProductEntity>()
            });
        }
        if (cartItems == null)
            return View(new ShoppingCartViewModel
            {
                images = nullImages,
                discounts = nullDiscounts,
                cartItems = nullCartItems,
                products = nullProducts
            });
        return View(new ShoppingCartViewModel
        {
            images = images,
            discounts = discounts,
            cartItems = cartItems,
            products = products
        });
    }
    private int? GetUserId()
        {
            return int.TryParse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId) ? userId : null;
        }
}
