using App.Data.Entities;

namespace SimoshStore;

public interface IBlogService
{ 
    Task<IEnumerable<BlogEntity>> GetLatestBlogs();
    Task<IEnumerable<BlogEntity>> GetRecentBlogs(); 
    Task<BlogEntity> GetRandomBlog();
    Task<BlogEntity> GetBlogByIdAsync(int blogId);
    Task<IEnumerable<BlogEntity>> GetAllBlogsAsync();
    Task<BlogEntity> CreateBlogAsync(BlogDTO blog);
    Task<BlogEntity> UpdateBlogAsync(BlogDTO blog, int id);
    Task DeleteBlogAsync(int blogId);
    Task AddTagToBlogAsync(int blogId, int tagId);
    Task AddCategoryToBlogAsync(int blogId, int categoryId);
    Task<IEnumerable<BlogCommentEntity>> GetComments(int blogId);
    Task<int> BlogCount();
}
    
