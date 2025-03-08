using App.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimoshStore;
using System.Linq;
using System.Threading.Tasks;

public class SearchViewComponent : ViewComponent
{
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;
    private readonly IDataRepository _Repository;

    public SearchViewComponent(IDataRepository dataRepository, ICategoryService categoryService, IProductService productService)
    {
        _categoryService = categoryService;
        _productService = productService;
        _Repository = dataRepository;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var categories = await _categoryService.ListAllCategories();
        var popularProducts = await _productService.PopularProducts();
        var discounts = _Repository.GetAll<DiscountEntity>();
        var images = await _Repository.GetAll<ProductImageEntity>().ToListAsync();
        foreach(var product in popularProducts)
        {
            if(images is not null)
            {
                product.Images = images.Where(i => i.ProductId == product.Id).ToList();
            }
            if(product.DiscountId!=null)
            {
                product.Discount = discounts.Where(d=>d.Id==product.Id).FirstOrDefault();
            }
            else
            {
                product.Discount = null;
            }
        }

        return View(
            new SearchBarViewModel
            {
                Categories = categories,
                Products = popularProducts,
            }
        );
    }
}
