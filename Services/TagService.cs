using App.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace SimoshStore;

public class TagService : ITagService
{
    private readonly IDataRepository _Repository;
    public TagService(IDataRepository Repository)
    {
        _Repository = Repository;
    }
    public List<RelBlogTagEntity> GetRelBlogTagEntities()
    {
        var relBlogTags = _Repository.GetAll<RelBlogTagEntity>().ToList();
        return relBlogTags;
    }
    public async Task<BlogTagEntity> GetTagByIdAsync(int tagId)
    {
        var tag = await _Repository.GetByIdAsync<BlogTagEntity>(tagId);
        if (tag == null)
        {
            return new BlogTagEntity();
        }
        return tag;
    }

    public async Task<IEnumerable<BlogTagEntity>> GetAllTagsAsync()
    {
        return await _Repository.GetAll<BlogTagEntity>().ToListAsync();
    }

    public async Task<BlogTagEntity> CreateTagAsync(BlogTagEntity tag)
    {
        await _Repository.AddAsync(tag);
        return tag;
    }

    public async Task<BlogTagEntity> UpdateTagAsync(BlogTagEntity tag)
    {
        await _Repository.UpdateAsync(tag);
        return tag;
    }

    public async Task DeleteTagAsync(int tagId)
    {
        var tag = await _Repository.GetByIdAsync<BlogTagEntity>(tagId);
        if (tag != null)
        {
            await _Repository.DeleteAsync<BlogTagEntity>(tagId);
        }
    }
}
