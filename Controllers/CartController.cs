using System.Security.Claims;
using System.Threading.Tasks;
using App.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimoshStore;

namespace MyApp.Namespace
{
    public class CartController : Controller
    {
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataRepository _Repository;
        public CartController(IUserService userService, IProductService productService, IOrderService orderService, IDataRepository Repository, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _Repository = Repository;
            _orderService = orderService;
            _productService = productService;
        }
        public async Task<IActionResult> ShopCart()
        {
            int userId = _userService.GetUserId();
            var cartItems = await _orderService.GetCartItemEntitiesByUserIdAsync(userId);
            var products = await _Repository.GetAll<ProductEntity>().ToListAsync();
            var discounts = await _Repository.GetAll<DiscountEntity>().ToListAsync();
            if (cartItems is not null)
            {
                foreach (var item in cartItems)
                {
                    item.Product = products.Where(p => p.Id == item.ProductId).FirstOrDefault();
                    if (item.Product.DiscountId is not null)
                    {
                        item.Product.Discount = discounts.Where(d => d.Id == item.Product.Id).FirstOrDefault();
                    }
                }
            }
            else{
                ViewData["Error"] = "Cart is empty";
            }

            return View(cartItems);

        }

        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            if (quantity == 0)
            {
                return BadRequest("Geçersiz quantity değeri.");
            }

            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            if (userIdClaim == null)
            {
                ViewData["AuthError"] = "You must log in for adding to cart";
                return RedirectToAction("Login", "Auth");
            }

            var userId = int.Parse(userIdClaim);

            var product = await _Repository.GetByIdAsync<ProductEntity>(productId);

            if (product == null)
            {
                return NotFound();
            }

            // Sepette zaten var mı kontrol et
            var existingCartItem = await _Repository.GetAll<CartItemEntity>()
                .FirstOrDefaultAsync(ci => ci.ProductId == productId && ci.UserId == userId);

            if (existingCartItem != null)
            {
                // Eğer ürün sepette varsa, miktarını artır
                existingCartItem.Quantity += quantity;
                await _Repository.UpdateAsync(existingCartItem);
            }
            else
            {
                // Eğer ürün sepette yoksa, yeni bir öğe ekle
                var cartItem = new CartItemEntity
                {
                    Quantity = quantity,
                    ProductId = productId,
                    UserId = userId
                };
                await _Repository.AddAsync(cartItem);
            }

            var refererUrl = _httpContextAccessor.HttpContext.Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(refererUrl))
            {
                return Redirect(refererUrl);
            }

            return RedirectToAction("ProductList", "Shop");
        }




        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            var userId = int.Parse(userIdClaim);
            var cartItem = _Repository.GetAll<CartItemEntity>()
            .FirstOrDefault(c => c.ProductId == productId && c.UserId == userId);
            if (cartItem == null)
            {
                return NotFound();
            }
            await _Repository.DeleteAsync<CartItemEntity>(cartItem.Id);
            return RedirectToAction("ProductList", "Shop");
        }
        public async Task<IActionResult> ClearCart()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim);

            var cartItems = _Repository.GetAll<CartItemEntity>()
                .Where(c => c.UserId == userId).ToList(); 

            foreach (var cartItem in cartItems)
            {
                await _Repository.DeleteAsync<CartItemEntity>(cartItem.Id);
            }

            return RedirectToAction("ShopCart", "Cart");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCart(int productId, int quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest("Geçersiz quantity.");
            }

            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim);

            var cartItem = await _Repository.GetAll<CartItemEntity>()
                .FirstOrDefaultAsync(ci => ci.ProductId == productId && ci.UserId == userId);

            if (cartItem == null)
            {
                return NotFound("Ürün sepette bulunamadı.");
            }

            cartItem.Quantity = quantity;
            await _Repository.UpdateAsync(cartItem);

            return RedirectToAction("Cart", "Cart");
        }

        public IActionResult GetCartItemCount()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            if (userIdClaim == null)
            {
                return Json(0); // Kullanıcı giriş yapmamışsa, 0 döndürüyoruz.
            }

            int userId = int.Parse(userIdClaim);
            var cartItems = _Repository.GetAll<CartItemEntity>().Where(x => x.UserId == userId).ToList();
            int cartCount = cartItems.Count();

            return Json(cartCount); 
        }
        [HttpGet]
        public async Task<IActionResult> CheckOut()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            if (userIdClaim == null)
            {
                ViewData["AuthError"] = "Bu sayfayı görmek için giriş yapmalısınız";
                return RedirectToAction("Login", "Auth");
            }
            int userId = int.Parse(userIdClaim);
            var cartItems = await _Repository.GetAll<CartItemEntity>()
                .Where(x => x.UserId == userId)
                .Include(ci => ci.Product)
                .ToListAsync();
            var user = await _Repository.GetByIdAsync<UserEntity>(userId);
            var products = await _productService.ListAllProducts();
            var discounts = await _Repository.GetAll<DiscountEntity>().ToListAsync();
            return View(new CheckOutViewModel
            {
                Discounts = discounts,
                cartItems = cartItems,
                User = user,
                products = products,
            });

        }
        [HttpPost]
        public async Task<IActionResult> CheckOut(CheckOutViewModel model)
        {
            int userId = _userService.GetUserId(); 
            var user = await _userService.GetUserByIdAsync(userId); 
            if (user == null)
            {
                ViewData["AuthError"] = "Bu sayfayı görmek için giriş yapmalısınız";
                return RedirectToAction("Login", "Auth"); 
            }

     
            return RedirectToAction("CreateOrder", "Order", new { userId = user.Id });
        }

    }

}
