using App.Data.Entities;

namespace SimoshStore;

public class HomeViewModel
{
    List<CartItemEntity> CartItems { get; set; } = null!;
}
