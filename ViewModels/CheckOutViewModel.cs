using App.Data.Entities;

namespace SimoshStore;

public class CheckOutViewModel
{
    public UserEntity User { get; set; } = null!;
    public List<CartItemEntity> cartItems { get; set; } = null!;
    public string OrderNote { get; set; } = null!;
    public int shipPrice { get; set; } = 0;
    public List<ProductEntity> products { get; set; } = null!;
    public List<DiscountEntity> Discounts { get; set; } = null!;
}
