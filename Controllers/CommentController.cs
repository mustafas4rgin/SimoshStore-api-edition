using System.Threading.Tasks;
using App.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimoshStore;

namespace MyApp.Namespace
{
    public class CommentController(IHttpClientFactory httpClientFactory) : BaseController
    {
        private HttpClient Client => httpClientFactory.CreateClient("Api.Data");
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ConfirmProductComment(int id)
        {
            var response = await Client.GetAsync($"/api/confirm-comment/{id}");

            if (!response.IsSuccessStatusCode)
            {
                SetErrorMessage("Failed to confirm comment");
                return RedirectToAction("NotFound", "Error");
            }

            return RedirectToAction("ProductCommentList", "Admin");
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProductComment(int id)
        {
            var response = await Client.GetAsync($"/api/delete/productcomment/{id}");

            if (!response.IsSuccessStatusCode)
            {
                SetErrorMessage("Failed to delete comment");
                return RedirectToAction("NotFound", "Error");
            }

            return RedirectToAction("ProductCommentList", "Admin");
        }
        [HttpPost]
        public async Task<IActionResult> AddBlogComment(BlogCommentDTO commentDTO)
        {

            var userId = GetUserId();
            if (userId == null)
            {
                return RedirectToAction("BlogPost", "Blog", new { id = commentDTO.BlogId, error = "User not found" });
            }
            var dto = new BlogCommentDTO
            {
                BlogId = commentDTO.BlogId,
                Name = commentDTO.Name,
                Email = commentDTO.Email,
                Comment = commentDTO.Comment,
                UserId = userId.Value,
            };

            var response = await Client.PostAsJsonAsync("/api/create/blogcomment", dto);

            if (!response.IsSuccessStatusCode)
            {
                SetErrorMessage("Failed to add comment");
                return RedirectToAction("NotFound", "Error");
            }

            return RedirectToAction("BlogPost", "Blog", new { id = commentDTO.BlogId });
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(int productId, string email, string commentText, int rating)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return RedirectToAction("ProductDetails", "Shop", new { id = productId, error = "User not found" });
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction("ProductDetails", "Shop", new { id = productId, error = "Invalid data" });
            }
            var dto = new ProductCommentDTO
            {
                ProductId = productId,
                UserId = userId.Value,
                StarCount = rating,
                Text = commentText
            };

            var response = await Client.PostAsJsonAsync("/api/create/productcomment", dto);

            if (!response.IsSuccessStatusCode)
            {
                SetErrorMessage("Failed to add comment");
                return RedirectToAction("NotFound", "Error");
            }

            SetSuccessMessage("Your comment is waiting for approval");  

            return RedirectToAction("ProductDetails", "Shop", new { id = productId });

        }

    }
}
