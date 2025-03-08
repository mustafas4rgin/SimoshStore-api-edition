using System.Threading.Tasks;
using App.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimoshStore;

namespace MyApp.Namespace
{
    public class CommentController : Controller
    {
        private readonly IUserService _userService;
        private readonly IDataRepository _Repository;
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService, IUserService userService, IDataRepository repository)
        {
            _commentService = commentService;
            _userService = userService;
            _Repository = repository;
        }
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> ConfirmProductComment(int id)
        {
            var result = await _commentService.ConfirmProductComment(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
            }
            return RedirectToAction("ProductCommentList","Admin");
        }
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> DeleteProductComment(int id)
        {
            await _Repository.DeleteAsync<ProductCommentEntity>(id);
            return RedirectToAction("AdminDashboard", "Admin");
        }
        [HttpPost]
        public async Task<IActionResult> AddBlogComment(BlogCommentDTO commentDTO)
        {
            ;
            int userId = _userService.GetUserId();
            if (userId == null)
            {
                return RedirectToAction("BlogPost", "Blog", new { id = commentDTO.BlogId, error = "User not found" });
            }
            var user = await _userService.GetUserByIdAsync(userId);
            var blogComment = new BlogCommentEntity
            {
                BlogId = commentDTO.BlogId,
                Name = user.FirstName + " " + user.LastName,
                Email = commentDTO.Email,
                Comment = commentDTO.Comment
            };
            await _Repository.AddAsync(blogComment);
            return RedirectToAction("BlogPost", "Blog", new { id = commentDTO.BlogId });
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(int productId, string email, string commentText, int rating)
        {
            int userId = _userService.GetUserId();

            if (userId == null)
            {
                return RedirectToAction("ProductDetails", "Shop", new { id = productId, error = "User not found" });
            }


            if (ModelState.IsValid)
            {
                var comment = new ProductCommentEntity
                {
                    ProductId = productId,
                    UserId = userId,
                    StarCount = rating,
                    Text = commentText
                };

                await _Repository.AddAsync(comment);
                TempData["CommentPendingApproval"] = "Your comment is awaiting approval.";
                return RedirectToAction("ProductDetails", "Shop", new { id = productId });
            }

            return RedirectToAction("ProductDetails", "Shop", new { id = productId });
        }

    }
}
