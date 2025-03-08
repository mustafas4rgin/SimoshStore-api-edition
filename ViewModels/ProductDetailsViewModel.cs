using App.Data.Entities;

namespace SimoshStore;

public class ProductDetailsViewModel
{
    public ProductEntity product { get; set; } = null!;
    public List<ProductImageEntity> images { get; set; } = null!;
    public int Discount { get; set; }
    public List<ProductCommentEntity> comments = null!;
    public CategoryEntity category { get; set; } = null!;
    public int Star { get; set; } = 0;
    public List<ProductEntity> productEntities { get; set; } = null!;
    public List<ProductEntity> orderedByCategoryProducts { get; set; } = null!;
    public List<ProductImageEntity> allImages { get; set; } = null!;
    public List<CategoryEntity> categories { get; set; } = null!;
}
