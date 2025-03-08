using App.Data.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace SimoshStore;

public class ProductService : IProductService
{
    private readonly IDataRepository _Repository;
    public ProductService(IDataRepository dataRepository)
    {
        _Repository = dataRepository;
    }
    public async Task<int> ProductCount()
    {
        return await _Repository.GetAll<ProductEntity>().CountAsync();
    }
    public async Task<IServiceResult> GettingProductImages(int id)
    {
        var images = await _Repository.GetAll<ProductImageEntity>().Where(x => x.ProductId == id).ToListAsync();
        if (images == null)
        {
            return new ServiceResult(false, "no images found");
        }
        return new ServiceResult(true, "images found");
    }
    public async Task<IServiceResult> CreateProductAsync(ProductDTO productDTO)
    {
        var newProduct = MappingHelper.MappingProduct(productDTO);
        var products = _Repository.GetAll<ProductEntity>();
        foreach (var product in products)
        {
            if (product.Name == newProduct.Name)
            {
                return new ServiceResult(false, "product already exists");
            }
        }
        await _Repository.AddAsync(newProduct);
        var imagesOfNewProduct = await GettingProductImages(newProduct.Id);
        if (!imagesOfNewProduct.Success)
        {
            await _Repository.AddAsync(new ProductImageEntity
            {
                ProductId = newProduct.Id,
                Url = "https://via.placeholder.com/150"
            });
        }
        return new ServiceResult(true, "product created successfully");
    }
    public async Task<IServiceResult> DeleteProductAsync(int id)
    {
        var product = await _Repository.GetByIdAsync<ProductEntity>(id);
        if (product == null)
        {
            return new ServiceResult(false, "product not found");
        }
        var productComments = await _Repository.GetAll<ProductCommentEntity>().Where(x => x.ProductId == id).ToListAsync();
        foreach (var comment in productComments)
        {
            await _Repository.DeleteAsync<ProductCommentEntity>(comment.Id);
        }
        var productImages = await _Repository.GetAll<ProductImageEntity>().Where(x => x.ProductId == id).ToListAsync();
        foreach (var image in productImages)
        {
            await _Repository.DeleteAsync<ProductImageEntity>(image.Id);
        }
        var orderItems = await _Repository.GetAll<OrderItemEntity>().Where(x => x.ProductId == id).ToListAsync();
        var orderIds = orderItems.Select(x => x.OrderId).Distinct().ToList();

        foreach (var orderItem in orderItems)
        {
            await _Repository.DeleteAsync<OrderItemEntity>(orderItem.Id);
        }
        var ordersToDelete = await _Repository.GetAll<OrderEntity>().Where(o => orderIds.Contains(o.Id)).ToListAsync();
        foreach (var order in ordersToDelete)
        {
            await _Repository.DeleteAsync<OrderEntity>(order.Id);
        }

        await _Repository.DeleteAsync<ProductEntity>(id);

        return new ServiceResult(true, "product deleted successfully");
    }

    public async Task<IServiceResult> UpdateProductAsync(ProductDTO productDTO, int id)
    {
        var product = await _Repository.GetByIdAsync<ProductEntity>(id);
        if (product is null)
        {
            return new ServiceResult(false, "product not found");
        }
        product.Name = productDTO.Name;
        product.Price = productDTO.Price;
        product.Description = productDTO.Description;
        product.StockAmount = productDTO.StockAmount;
        product.Enabled = productDTO.Enabled;
        product.CategoryId = productDTO.CategoryId;
        product.DiscountId = productDTO.DiscountId;
        product.SellerId = productDTO.SellerId;
        await _Repository.UpdateAsync(product);
        return new ServiceResult(true, "product updated successfully");
    }
    public async Task<IServiceResult> GetAllProductsAsync()
    {
        if (_Repository.GetAll<ProductEntity> == null)
        {
            return new ServiceResult(false, "no products found");
        }
        return new ServiceResult(true, "products found");
    }
    public async Task<IServiceResult> GetProductAsync(ProductEntity product)
    {
        if (await _Repository.GetByIdAsync<ProductEntity>(product.Id) == null)
        {
            return new ServiceResult(false, "product not found");
        }
        return new ServiceResult(true, "product found");
    }
    public async Task<IServiceResult> OrderProductsByCategory(List<ProductEntity> products)
    {
        products = products.OrderBy(x => x.CategoryId).ToList();
        if (products == null)
        {
            return new ServiceResult(false, "no products found");
        }
        return new ServiceResult(true, "products found");
    }
    public ProductEntity GetRandomProduct()
    {
        
        var products = _Repository.GetAll<ProductEntity>().ToList();

        
        if (!products.Any())
        {
            return null; 
        }

        
        var randomProduct = products[new Random().Next(products.Count)];

        return randomProduct;
    }
    public async Task<List<ProductEntity>> PopularProducts()
    {
        var popularProducts = _Repository.GetAll<ProductEntity>()
                                                 .Select(p => new
                                                 {
                                                     Product = p,
                                                     CommentCount = _Repository.GetAll<ProductCommentEntity>().Count(c => c.ProductId == p.Id),
                                                     AverageRating = _Repository.GetAll<ProductCommentEntity>().Where(c => c.ProductId == p.Id).Average(c => (double?)c.StarCount) ?? 0
                                                 })
                                                 .OrderByDescending(p => p.CommentCount + p.AverageRating * 100)
                                                 .Take(8)
                                                 .ToList();

        var products = popularProducts.Select(p => p.Product).ToList();

        return products;
    }
    public async Task<List<ProductEntity>> ListAllProducts()
    {
        var products = await _Repository.GetAll<ProductEntity>().ToListAsync();
        if (products is not null)
            return products;
        return new List<ProductEntity>();
    }

}