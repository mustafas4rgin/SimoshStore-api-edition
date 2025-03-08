using App.Data.Entities;

namespace SimoshStore;

public class IndexViewModel
{
    public List<CategoryEntity> categories { get; set; } = null!;
    public List<ProductEntity> products { get; set; } = null!;
    public List<ProductImageEntity> images { get; set; } = null!;
    public List<DiscountEntity> discounts { get; set; } = null!;
    public List<ProductEntity> randomProducts { get; set; } = null!;
    public List<ProductEntity> popularProducts {get; set;} = null!;
    public List<BlogEntity> blogEntities {get; set;} = null!;
}
