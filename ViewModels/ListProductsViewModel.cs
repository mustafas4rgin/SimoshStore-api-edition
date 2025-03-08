using App.Data.Entities;
using SimoshStore;

namespace MyApp.Namespace
{
    internal class ListProductsViewModel
    {
        public List<ProductEntity> productEntities { get; set; }
        public List<CategoryEntity> categories { get; set; }
        public List<ProductImageEntity> images { get; set; }
        public List<ProductCommentEntity> comments { get; set; }
        public List<DiscountEntity> discounts { get; set; }
        public int CurrentPage { get; set; }
        public int TotalProductCount { get; set; }
        public int TotalPages { get; set; }
        public List<int> SelectedCategoryIds { get; set; } // Seçilen kategoriler
        public decimal PriceMin { get; set; } // Minimum Price
        public decimal PriceMax { get; set; } // Maximum Price
        public string DzSearch { get; set; } // Arama kelimesi
    }
}
