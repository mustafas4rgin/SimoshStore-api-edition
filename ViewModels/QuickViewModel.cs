using App.Data.Entities;

namespace SimoshStore;

public class QuickViewModel
{
    public ProductEntity product { get; set; } = null!;
    public List<ProductImageEntity> images { get; set; } = null!;
}
