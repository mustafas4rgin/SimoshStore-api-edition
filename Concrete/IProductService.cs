using App.Data.Entities;

namespace SimoshStore;

public interface IProductService
{
    Task<IServiceResult> CreateProductAsync(ProductDTO product);
    Task<IServiceResult> UpdateProductAsync(ProductDTO productDTO, int id);
    Task<IServiceResult> DeleteProductAsync(int id);
    Task<IServiceResult> GetAllProductsAsync();
    Task<IServiceResult> GetProductAsync(ProductEntity product);
    Task<IServiceResult> GettingProductImages(int id);
    Task<IServiceResult> OrderProductsByCategory(List<ProductEntity> products);
    ProductEntity GetRandomProduct();
    Task<List<ProductEntity>> PopularProducts();
    Task<List<ProductEntity>> ListAllProducts();
    Task<int> ProductCount();
}
