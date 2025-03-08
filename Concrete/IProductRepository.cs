using App.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace SimoshStore;

public interface IProductRepository
{
    Task<List<ProductEntity>> GetAllProductsAsync();
    Task<ProductEntity> GetProductByIdAsync(int id);
    Task AddProduct(ProductEntity product);
    Task DeleteProduct(ProductEntity product);
    Task UpdateProduct(ProductEntity product);
}
