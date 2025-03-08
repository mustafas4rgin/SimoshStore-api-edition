using App.Data.Entities;

namespace SimoshStore;

public class ShoppingCartViewModel
{
    public List<CartItemEntity> cartItems { get; set; } = null!;
    public List<ProductEntity> products { get; set; } = null!;
    public List<DiscountEntity> discounts {get; set;} = null!;
    public List<ProductImageEntity> images {get; set;} = null!;
}
