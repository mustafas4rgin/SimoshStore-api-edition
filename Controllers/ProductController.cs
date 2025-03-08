using System.Threading.Tasks;
using App.Data.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimoshStore;

namespace MyApp.Namespace
{
    [Authorize(Roles = "admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IDataRepository _Repository;
        public ProductController(IProductService productService, IDataRepository repository)
        {
            _Repository = repository;
            _productService = productService;
        }
        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _Repository.GetAll<ProductEntity>().ToListAsync();
            var result = await _productService.GetAllProductsAsync();
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return View("ProductList", products);
        }
        [HttpGet("GetProduct")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _Repository.GetByIdAsync<ProductEntity>(id);
            var result = await _productService.GetProductAsync(product);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return View(product);
        }
        public async Task<IActionResult> CreateProduct()
        {
            // Kategorileri ViewBag'e gönderiyoruz
            var categories = await _Repository.GetAll<CategoryEntity>().ToListAsync();
            ViewBag.Categories = categories.ToList();

            return View(new ProductDTO());
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDTO productDTO)
        {

            var result = await _productService.CreateProductAsync(productDTO);
            if (result.Success)
            {
                return RedirectToAction("ProductList", "Admin");
            }
            else
            {
                ViewBag.Error = result.Message;
                var categories = await _Repository.GetAll<CategoryEntity>().ToListAsync();
                ViewBag.Categories = categories.ToList();
            }

            return RedirectToAction("ProductList", "Admin");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var product = await _Repository.GetByIdAsync<ProductEntity>(id);
            if (product == null)
            {
                return NotFound();
            }

            var categories = await _Repository.GetAll<CategoryEntity>().ToListAsync();
            var discounts = await _Repository.GetAll<DiscountEntity>().ToListAsync();

            var productDTO = new ProductDTO
            {
                SellerId = product.SellerId,
                CategoryId = product.CategoryId,
                DiscountId = product.DiscountId,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                StockAmount = product.StockAmount,
                Enabled = product.Enabled
            };

            ViewBag.Categories = categories;
            ViewBag.Discounts = discounts;

            return View(productDTO);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(int id, ProductDTO productDTO)
        {

            var result = await _productService.UpdateProductAsync(productDTO, id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View(productDTO);
            }

            return RedirectToAction("AdminDashboard", "Admin");

        }
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return RedirectToAction("AdminDashboard", "Admin");
        }


    }
}
