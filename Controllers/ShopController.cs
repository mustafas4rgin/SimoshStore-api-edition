using App.Data.Entities;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimoshStore;

namespace MyApp.Namespace
{
    public class ShopController(IHttpClientFactory httpClientFactory) : Controller
    {
        private HttpClient Client => httpClientFactory.CreateClient("Api.Data");
        public async Task<IActionResult> ProductList(int page = 1, List<int> selectedCategoryIds = null, decimal? priceMin = null, decimal? priceMax = null, string dzSearch = null)
    {
        int pageSize = 4;
        int skip = (page - 1) * pageSize;

        var response = await Client.GetAsync("/api/product-list");

        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var listProductsViewModel = await response.Content.ReadFromJsonAsync<ListProductsViewModel>();

        var query = listProductsViewModel.productEntities.AsQueryable();

        if (selectedCategoryIds != null && selectedCategoryIds.Any())
        {
            query = query.Where(p => selectedCategoryIds.Contains(p.CategoryId));
        }

        if (!string.IsNullOrEmpty(dzSearch))
        {
            query = query.Where(p => p.Name.ToLower().Contains(dzSearch.ToLower()));
        }


        var products = query.Skip(skip).Take(pageSize).ToList();
        int totalProducts = query.Count();
        int totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

        var viewModel = new ListProductsViewModel
        {
            productEntities = products,
            CurrentPage = page,
            TotalProductCount = totalProducts,
            TotalPages = totalPages,
            SelectedCategoryIds = selectedCategoryIds ?? new List<int>(), 
            DzSearch = dzSearch 
        };

        return View(viewModel);
    }





        [HttpGet]
        public async Task<IActionResult> ProductDetails(int? id)
        {
            var response = await Client.GetAsync($"/api/products/{id}");

            if(!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var model = await response.Content.ReadFromJsonAsync<ProductDetailsViewModel>();

            return View(model);
        }

    }
}
