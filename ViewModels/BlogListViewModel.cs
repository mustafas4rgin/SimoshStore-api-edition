using App.Data.Entities;

namespace SimoshStore;

public class BlogListViewModel
{
    public IEnumerable<BlogEntity> blogEntities { get; set; } = null!;
    public IEnumerable<BlogCategoryEntity> blogCategoryEntities { get; set; } = null!;
    public List<BlogCommentEntity> blogCommentEntities { get; set; } = null!;
    public List<string> Dates { get; set; } = null!;
}
