using App.Data.Entities;

namespace SimoshStore.Controllers;

internal class AboutUsViewModel
{
    public List<OrderEntity> orders { get; set; } = null!;
    public List<ProductCommentEntity> productComments { get; set; } = null!;
    public List<UserEntity> users { get; set; } = null!;
}