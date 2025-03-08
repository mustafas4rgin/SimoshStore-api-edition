using App.Data.Entities;

namespace SimoshStore;

public class OrderListViewModel
{
    public List<OrderEntity> orders { get; set; } = null!;
    public List<OrderItemEntity> orderItems { get; set; } = null!;
    public int CurrentPage { get; set; } = 0;
    public int TotalPages { get; set; } = 0;
} 
