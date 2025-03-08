using App.Data.Entities;

namespace SimoshStore;

public class ReviewViewModel
{
    public List<ProductCommentEntity> ProductComments { get; set; } = null!;
    public List<BlogCommentEntity> BlogComments { get; set; } = null!;
}
