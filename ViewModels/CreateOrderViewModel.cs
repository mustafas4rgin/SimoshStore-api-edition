using App.Data.Entities;

namespace SimoshStore;

public class CreateOrderViewModel
{
    public List<CartItemEntity> CartItems { get; set; } = null!;
    public UserEntity User { get; set; } = null!;
}
