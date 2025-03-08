using App.Data.Entities;

namespace SimoshStore;

public class ProfileViewModel
{
    public List<OrderEntity> orders { get; set; } = null!;
    public UserEntity user { get; set; } = null!;
    public List<ProductCommentEntity> productComments { get; set; } = null!;
    public List<OrderItemEntity> orderItems { get; set; } = null!;
}
