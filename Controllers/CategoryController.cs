using App.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimoshStore;

namespace MyApp.Namespace
{
    [Authorize(Roles = "admin")]
    public class CategoryController : Controller
    {
        // GET: CategoryControlle

        private readonly ICategoryService _categoryService;
        private readonly IDataRepository _Repository;

        public CategoryController(ICategoryService categoryService, IDataRepository Repository)
        {
            _categoryService = categoryService;
            _Repository = Repository;
        }
        [HttpGet]
        public async Task<IActionResult> ListCategories()
        {
            var categories = _Repository.GetAll<CategoryEntity>();
            var result = await _categoryService.GetAllCategoriesAsync();
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return View(categories);
        }
        [HttpGet]
        public async Task<IActionResult> CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDTO categoryDTO)
        {
            var result = await _categoryService.CreateCategoryAsync(categoryDTO);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return RedirectToAction("CategoryList");
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            var category = await _Repository.GetByIdAsync<CategoryEntity>(id);
            if (category == null)
            {
                ViewBag.Error = "Category not found";
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
            var result = await _categoryService.UpdateCategoryAsync(categoryDTO, id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return RedirectToAction("CategoryList","Admin");
        }
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            var categories = await _Repository.GetAll<CategoryEntity>().ToListAsync();
            var category = await _Repository.GetByIdAsync<CategoryEntity>(id);
            if (category == null)
            {
                ViewBag.Error = "Category not found";
                return RedirectToAction("CategoryList","Admin");
            }
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return RedirectToAction("CategoryList","Admin");
            }
            return RedirectToAction("CategoryList","Admin");
        }


    }
}
