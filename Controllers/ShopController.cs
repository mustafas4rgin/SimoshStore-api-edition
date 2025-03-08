using App.Data.Entities;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimoshStore;

namespace MyApp.Namespace
{
    public class ShopController : Controller
    {
        private readonly IDataRepository _Repository;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        public ShopController(IUserService userService,IDataRepository Repository, IProductService productService)
        {
            _userService = userService;
            _Repository = Repository;
            _productService = productService;
        }
        public async Task<IActionResult> ProductList(int page = 1, List<int> selectedCategoryIds = null, decimal? priceMin = null, decimal? priceMax = null, string dzSearch = null)
        {
            int pageSize = 4;
            int skip = (page - 1) * pageSize;

            var comments = await _Repository.GetAll<ProductCommentEntity>().Where(c => c.IsConfirmed).ToListAsync();
            var discounts = await _Repository.GetAll<DiscountEntity>().ToListAsync();
            var categories = await _Repository.GetAll<CategoryEntity>().ToListAsync();
            var images = await _Repository.GetAll<ProductImageEntity>().ToListAsync();

            var query = _Repository.GetAll<ProductEntity>().AsQueryable();

            if (selectedCategoryIds != null && selectedCategoryIds.Any())
            {
                query = query.Where(p => selectedCategoryIds.Contains(p.CategoryId));
            }

            if (!string.IsNullOrEmpty(dzSearch))
            {
                query = query.Where(p =>     p.Name.ToLower().Contains(dzSearch.ToLower())); // Büyük/küçük harf duyarsız olmayacak
            }

            var products = await query.Skip(skip).Take(pageSize).ToListAsync();
            int totalProducts = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
            
            var viewModel = new ListProductsViewModel
            {
                comments = comments,
                discounts = discounts,
                productEntities = products,
                categories = categories,
                images = images,
                CurrentPage = page,
                TotalProductCount = totalProducts,
                TotalPages = totalPages,
                SelectedCategoryIds = selectedCategoryIds ?? new List<int>(), 
                PriceMin = priceMin ?? 0,  
                PriceMax = priceMax ?? 1000,
                DzSearch = dzSearch 
            };

            return View(viewModel);
        }





        [HttpGet]
        public async Task<IActionResult> ProductDetails(int? id)
        {
            string PhotoUrl = await PhotoHelper.GetRandomPhotoUrlAsync();
            ViewBag.PhotoUrl = PhotoUrl;
            if (id == null)
            {
                return NotFound();
            }
            var Categories = await _Repository.GetAll<CategoryEntity>().ToListAsync();
            var Products = await _Repository.GetAll<ProductEntity>().ToListAsync();
            
            var Product = await _Repository.GetByIdAsync<ProductEntity>(id.Value);
            if (Product == null)
            {
                return NotFound();
            }
            var Category = await _Repository.GetByIdAsync<CategoryEntity>(Product.CategoryId);
            var Images = await _Repository.GetAll<ProductImageEntity>().Where(x => x.ProductId == id.Value).ToListAsync();
            var comments = await _Repository.GetAll<ProductCommentEntity>().Where(x => x.ProductId == id.Value).ToListAsync();
            var Comments = comments.Where(s=>s.IsConfirmed==true).ToList();
            var OrderedByCategoryProducts = Products;
            var AllImages = await _Repository.GetAll<ProductImageEntity>().ToListAsync();
            await _productService.OrderProductsByCategory(OrderedByCategoryProducts);
            int stars = 0;
            foreach (var comment in Comments)
            {
                comment.User = await _userService.GetUserByIdAsync(comment.UserId);
                stars += comment.StarCount;
            }
            if (Comments.Count != 0)
                stars = stars / Comments.Count;
            else
            {
                stars = 0;
            }
            var VM = new ProductDetailsViewModel
            {
                categories = Categories,
                allImages = AllImages,
                productEntities = Products,
                orderedByCategoryProducts = OrderedByCategoryProducts,
                Star = stars,
                category = Category,
                comments = Comments,
                product = Product,
                images = Images
            };
            return View(VM);
        }

    }
}
