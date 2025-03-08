using App.Data.Entities;

namespace SimoshStore;

public interface ITagService
{
    Task<BlogTagEntity> GetTagByIdAsync(int tagId);
    Task<IEnumerable<BlogTagEntity>> GetAllTagsAsync();
    Task<BlogTagEntity> CreateTagAsync(BlogTagEntity tag);
    Task<BlogTagEntity> UpdateTagAsync(BlogTagEntity tag);
    Task DeleteTagAsync(int tagId);
    public List<RelBlogTagEntity> GetRelBlogTagEntities();
}
