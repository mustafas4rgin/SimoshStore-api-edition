using System.Threading.Tasks;
using App.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SimoshStore.Controllers
{
    [Authorize(Roles = "admin")] 
    public class AdminController : Controller
    {
        private readonly IDataRepository _repository;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IBlogService _blogService;
        private readonly IEmailService _emailService;
        private readonly IBlogCategoryService _blogCategoryService;
        private readonly ICommentService _commentService;
        private readonly IProfileService _profileService;


        public AdminController(IOrderService orderService, IUserService userService, IProfileService profileService, ICommentService commentService, IBlogCategoryService blogCategoryService, IEmailService emailService, IBlogService blogService, IDataRepository repository, IProductService productService, ICategoryService categoryService)
        {
            _profileService = profileService;
            _commentService = commentService;
            _blogCategoryService = blogCategoryService;
            _emailService = emailService;
            _blogService = blogService;
            _orderService = orderService;
            _userService = userService;
            _repository = repository;
            _productService = productService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> AdminDashboard()
        {
            var LatestOrders = await _orderService.GetLatestOrders();
            var LatestComments = await _commentService.GetLatestComments();
            var viewModel = new AdminDashboardViewModel
            {
                TotalCategories = await _categoryService.CategoryCount(),
                TotalProducts = await _productService.ProductCount(),
                TotalOrders = await _orderService.OrderCount(),
                TotalUsers = await _userService.UserCount(),
                LatestOrders = LatestOrders.ToList(),
                LatestComments = LatestComments,
                LatestBlogs = await _blogService.GetLatestBlogs()
            };

            return View(viewModel);
        }
        public async Task<IActionResult> OrderList()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders.ToList());
        }
        public async Task<IActionResult> BlogCommentList()
        {
            var comments = await _repository.GetAll<BlogCommentEntity>().ToListAsync();
            return View(comments);
        }
        public IActionResult ManageUsers(int page = 1, int pageSize = 10)
        {
            var users = _repository.GetAll<UserEntity>();

            var totalUsers = users.Count();

            var viewModel = new UserListViewModel
            {
                Users = users.Select(user => new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.Phone,
                    RoleId = user.RoleId,
                    Enabled = user.Enabled
                }).ToList(),
                TotalUsersCount = totalUsers,
                CurrentPage = page,
                PageSize = pageSize
            };

            return View(viewModel);
        }
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                ViewBag.Error = "Order not found";
                return View();
            }
            await _orderService.DeleteOrderAsync(orderId);
            return RedirectToAction("OrderList", "Admin");
        }

        public IActionResult ViewReports()
        {
            return View("UnderConstruction", "Error");
        }
        public async Task<IActionResult> ProductCommentList()
        {
            var comments = await _repository.GetAll<ProductCommentEntity>().ToListAsync();
            foreach (var comment in comments)
            {
                comment.User = await _repository.GetByIdAsync<UserEntity>(comment.UserId);
                comment.Product = await _repository.GetByIdAsync<ProductEntity>(comment.ProductId);
            }
            return View(comments);
        }
        public async Task<IActionResult> ProductList()
        {
            var products = _repository.GetAll<ProductEntity>().ToList();
            var categories = await _categoryService.ListAllCategories();
            var result = await _productService.GetAllProductsAsync();
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            foreach (var product in products)
            {
                product.Discount = await _repository.GetAll<DiscountEntity>().FirstOrDefaultAsync(x => x.Id == product.DiscountId);
                product.Category = categories.ToList().FirstOrDefault(x => x.Id == product.CategoryId);
            }
            return View("ProductList", products);
        }
        public async Task<IActionResult> CategoryList()
        {
            var categories = _repository.GetAll<CategoryEntity>().ToList();
            var result = await _categoryService.GetAllCategoriesAsync();
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View("CategoryList", categories);
            }
            return View(categories);
        }
        public async Task<IActionResult> ListBlogCategories()
        {
            var blogCategories = await _repository.GetAll<BlogCategoryEntity>().ToListAsync();
            return View(blogCategories);
        }
        public async Task<IActionResult> ListBlogs()
        {
            var blogs = await _repository.GetAll<BlogEntity>().ToListAsync();
            foreach(var blog in blogs)
            {
                blog.User = await _repository.GetByIdAsync<UserEntity>(blog.UserId);
            }
            return View(blogs);
        }

        public async Task<IActionResult> OrderDetails(int id)
        {
            var orders = await _orderService.GetAllOrdersAsync();
            var order = orders
                .Where(o => o.Id == id)
                .FirstOrDefault();



            if (order == null)
            {
                return NotFound();
            }
            var orderItems = _repository.GetAll<OrderItemEntity>().ToList().Where(o => o.OrderId == order.Id);
            if (orderItems is null)
            {
                return NotFound();
            }
            order.OrderItems = orderItems.ToList();
            foreach (var item in orderItems)
            {
                var orderItemOrder = _repository.GetAll<OrderEntity>().Where(o => o.Id == item.OrderId).FirstOrDefault();
                if (orderItemOrder is not null)
                {
                    item.Order = orderItemOrder;
                }
                var product = _repository.GetAll<ProductEntity>().Where(p => p.Id == item.ProductId).FirstOrDefault();
                if (product is not null)
                {
                    item.Product = product;
                }

            }
            return View(order);
        }
    }
}

