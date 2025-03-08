using App.Data.Entities;

namespace SimoshStore;

public interface IOrderService
{
    public Task<int> OrderCount();
    public Task<IEnumerable<OrderItemEntity>> GetAllOrderItems();
    public Task<IEnumerable<OrderItemEntity>> GetOrderItemsByOrderIdAsync(int orderId);
    public Task<OrderEntity> GetOrderByIdAsync(int orderId);
    public Task<IEnumerable<OrderEntity>> GetAllOrdersAsync();
    public Task<OrderEntity> CreateOrderAsync(OrderDTO dto);
    public Task<OrderEntity> UpdateOrderAsync(OrderDTO dto);
    public Task DeleteOrderAsync(int orderId);
    public Task<IEnumerable<OrderEntity>> GetOrdersByUserIdAsync(int userId);
    public Task<IServiceResult> AddingOrderItemsAsync(OrderEntity order);
    public Task<IEnumerable<CartItemEntity>> GetCartItemEntitiesByUserIdAsync(int userId);
    public Task<IEnumerable<OrderEntity>> GetLatestOrders();

}
