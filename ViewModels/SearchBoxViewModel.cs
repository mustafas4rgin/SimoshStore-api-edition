using App.Data.Entities;

namespace SimoshStore;

public class SearchBoxViewModel
{
    public List<CategoryEntity> Categories { get; set; }
    public List<ProductEntity> FeaturedProducts { get; set; }
    public List<int> SelectedCategoryIds { get; set; }
    public string DzSearch { get; set; } = "";
}
