using App.Data.Entities;
using SimoshStore;

namespace MyApp.Namespace
{
    internal class ListProductsViewModel
    {
        public List<ProductEntity> productEntities { get; set; }
        public int CurrentPage { get; set; }
        public int TotalProductCount { get; set; }
        public int TotalPages { get; set; }
        public List<int> SelectedCategoryIds { get; set; } 
        public string DzSearch { get; set; } 
        int Star { get; set; } = 0;
    }
}
