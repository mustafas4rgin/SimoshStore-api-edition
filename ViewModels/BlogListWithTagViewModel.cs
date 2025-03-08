using App.Data.Entities;

namespace SimoshStore;

public class BlogListWithTagViewModel
{
    public List<BlogEntity> blogEntities { get; set; } = null!;
    public List<BlogCategoryEntity> blogCategoryEntities { get; set; } = null!;
    public List<BlogCommentEntity> blogCommentEntities { get; set; } = null!;
    public List<CategoryEntity> categories { get; set; } = null!;
    public List<ProductEntity> products { get; set; } = null!;
    public BlogTagEntity tag { get; set; } = null!;
}
