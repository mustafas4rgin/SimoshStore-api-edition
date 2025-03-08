using System.Threading.Tasks;
using App.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimoshStore;

namespace MyApp.Namespace
{
    public class BlogController : Controller
    {
        ITagService _tagService;
        IDataRepository _Repository;
        IBlogService _blogService;
        IBlogCategoryService _blogCategoryService;
        ICategoryService _categoryService;
        IProductService _productService;
        IUserService _userService;
        public BlogController(IUserService userService, ITagService tagService, ICategoryService categoryService, IProductService productService, IDataRepository dataRepository, IBlogService blogService, IBlogCategoryService blogCategoryService)
        {
            _Repository = dataRepository;
            _blogCategoryService = blogCategoryService;
            _blogService = blogService;
            _productService = productService;
            _categoryService = categoryService;
            _tagService = tagService;
            _userService = userService;
        }
        public async Task<IActionResult> BlogPost(int id)
        {
            var randomBlog = await _blogService.GetRandomBlog();
            var blogEntity = await _blogService.GetBlogByIdAsync(id);
            if (blogEntity is null)
            {
                return RedirectToAction("UnderConstruction", "Error");
            }
            var blogEntities = await _blogService.GetAllBlogsAsync();
            var blogComments = await _blogService.GetComments(id);
            var relBlogTags = await _Repository.GetAll<RelBlogTagEntity>().ToListAsync();
            var user = await _Repository.GetByIdAsync<UserEntity>(blogEntity.UserId);
            List<BlogTagEntity> tags = new List<BlogTagEntity>();
            foreach (var relBlogTag in relBlogTags)
            {
                var tag = await _tagService.GetTagByIdAsync(relBlogTag.TagId);
                tags.Add(tag);
            }
            if (user is null)
            {
                user = new UserEntity();
            }
            return View(new BlogPostViewModel
            {
                randomBlog = randomBlog,
                blog = blogEntity,
                blogs = blogEntities,
                blogComments = blogComments,
                user = user,
                quote = QuoteHelper.GenerateQuote(),
                tags = tags

            });

        }
        public async Task<IActionResult> DeleteBlog(int id)
        {
            await _blogService.DeleteBlogAsync(id);
            return RedirectToAction("ListBlogs","Admin");
        }
        public async Task<IActionResult> BlogList()
        {
            var blogEntities = await _blogService.GetAllBlogsAsync();
            var blogCategoryEntities = await _blogCategoryService.GetAllCategoriesAsync();
            return View(new BlogListViewModel
            {
                blogCategoryEntities = blogCategoryEntities,
                blogEntities = blogEntities,

            });
        }
        public async Task<IActionResult> BlogListWithTag(BlogTagEntity blogTagEntity)
        {
            return RedirectToAction("UnderConstruction", "Error");
        }
        public async Task<IActionResult> RecentBlogs()
        {
            var blogEntities = await _blogService.GetRecentBlogs();
            return View(blogEntities);
        }
        public async Task<IActionResult> CreateBlog()
        {
            int userId = _userService.GetUserId();
            var user = await _userService.GetUserByIdAsync(userId);
            if (user is null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(new BlogDTO{
                userId = userId
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateBlog(BlogDTO blogDTO)
        {
            var blog = await _blogService.CreateBlogAsync(blogDTO);
            return RedirectToAction("BlogList");
        }
    }
}
