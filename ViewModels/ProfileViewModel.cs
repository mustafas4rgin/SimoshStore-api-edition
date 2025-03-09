using App.Data.Entities;

namespace SimoshStore;

public class ProfileViewModel
{
    public List<OrderEntity> Orders { get; set; } = null!;
    public UserEntity User { get; set; } = null!;
    public List<ProductCommentEntity> ProductComments { get; set; } = null!;
    public decimal TotalPrice { get; set; }
}
