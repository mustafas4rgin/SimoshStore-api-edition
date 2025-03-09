using App.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimoshStore;

namespace MyApp.Namespace
{
    [Authorize(Roles = "admin")]
    public class CategoryController(IHttpClientFactory httpClientFactory) : BaseController
    {
        private HttpClient Client => httpClientFactory.CreateClient("Api.Data");
        // GET: CategoryControlle

        [HttpGet]
        public async Task<IActionResult> ListCategories()
        {
            var response = await Client.GetAsync("api/categories");

            if (!response.IsSuccessStatusCode)
            {
                SetErrorMessage("Failed to load categories");
                return View();
            }

            var categories = await response.Content.ReadFromJsonAsync<List<CategoryEntity>>();

            return View(categories);
        }
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDTO categoryDTO)
        {
            var response = await Client.PostAsJsonAsync("api/create/category", categoryDTO);

            if (!response.IsSuccessStatusCode)
            {
                SetErrorMessage("Failed to create category");
                return View();
            }
            SetSuccessMessage("Category created successfully");

            return RedirectToAction("CategoryList");
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            var response = await Client.GetAsync($"api/categories/{id}");

            if (!response.IsSuccessStatusCode)
            {
                SetErrorMessage("Failed to load category");
                return View();
            }

            var category = await response.Content.ReadFromJsonAsync<CategoryEntity>();

            if (category == null)
            {
                SetErrorMessage("Category not found");
                return View();
            }
            var categoryDTO = new CategoryDTO
            {
                Name = category.Name,
                Color = category.Color,
                IconCssClass = category.IconCssClass
            };
            return View(categoryDTO);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(CategoryDTO categoryDTO, int id)
        {
            var response = await Client.PutAsJsonAsync($"api/update/category/{id}", categoryDTO);

            if (!response.IsSuccessStatusCode)
            {
                SetErrorMessage("Failed to update category");
                return View();
            }

            SetSuccessMessage("Category updated successfully");

            return RedirectToAction("CategoryList","Admin");
        }
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            var response = await Client.DeleteAsync($"api/delete/category/{id}");

            if (!response.IsSuccessStatusCode)
            {
                SetErrorMessage("Failed to delete category");
                return RedirectToAction("CategoryList","Admin");
            }

            SetSuccessMessage("Category deleted successfully");
            
            return RedirectToAction("CategoryList","Admin");
        }


    }
}
