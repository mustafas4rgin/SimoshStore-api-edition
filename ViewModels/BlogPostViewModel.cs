using App.Data.Entities;

namespace SimoshStore;

public class BlogPostViewModel
{
    public IEnumerable<BlogCommentEntity> blogComments { get; set; } = null!;
    public IEnumerable<BlogEntity> blogs { get; set; } = null!;
    public UserEntity user { get; set; } = null!;
    public BlogEntity blog { get; set; } = null!;
    public string quote { get; set; } = null!;
    public BlogEntity randomBlog { get; set; } = null!;
    public List<BlogTagEntity> tags { get; set; } = null!;
}
