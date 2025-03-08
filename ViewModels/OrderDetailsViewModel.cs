using App.Data.Entities;

namespace SimoshStore;

public class OrderDetailsViewModel
{
    public List<OrderItemEntity> orderItems { get; set; } = null!;
    public ProductEntity product { get; set; } = null!;
    public OrderEntity order { get; set; } = null!;

}
