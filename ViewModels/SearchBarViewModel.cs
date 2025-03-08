using App.Data.Entities;

namespace SimoshStore;

public class SearchBarViewModel
{
    public List<ProductEntity> Products { get; set; } = null!;
    public List<CategoryEntity> Categories { get; set; } = null!;
}
