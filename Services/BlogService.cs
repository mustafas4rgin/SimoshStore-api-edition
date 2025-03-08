using System.Threading.Tasks;
using App.Data.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace SimoshStore;

public class BlogService : IBlogService
{
    IDataRepository _Repository;
    public BlogService(IDataRepository repository)
    {
        _Repository = repository;
    }
    public async Task<IEnumerable<BlogEntity>> GetRecentBlogs()
    {
        return await _Repository.GetAll<BlogEntity>()
            .OrderByDescending(b => b.CreatedAt)
            .Take(3)
            .ToListAsync();
    }
    public async Task<IEnumerable<BlogCommentEntity>> GetComments(int blogId)
    {
        return await _Repository.GetAll<BlogCommentEntity>()
            .Where(bc => bc.BlogId == blogId)
            .ToListAsync();
    }
    public async Task<IEnumerable<BlogEntity>> GetLatestBlogs()
    {
        return await _Repository.GetAll<BlogEntity>()
            .OrderByDescending(b => b.CreatedAt)
            .Take(3)
            .ToListAsync();
    }
    public async Task<BlogEntity> GetRandomBlog()
    {
        var blogs = await _Repository.GetAll<BlogEntity>().ToListAsync();
        var random = new Random();
        var randomBlog = blogs[random.Next(blogs.Count)];
        return randomBlog;
    }
    public async Task<int> BlogCount()
    {
        return await _Repository.GetAll<BlogEntity>().CountAsync();
    }
    public async Task<BlogEntity> GetBlogByIdAsync(int blogId)
    {
        return await _Repository.GetAll<BlogEntity>()
            .Include(b => b.BlogTags)
            .ThenInclude(bt => bt.Tag)
            .Include(b => b.BlogCategories)
            .ThenInclude(bc => bc.Category)
            .FirstOrDefaultAsync(b => b.Id == blogId);
    }

    public async Task<IEnumerable<BlogEntity>> GetAllBlogsAsync()
    {
        return await _Repository.GetAll<BlogEntity>()
            .Include(b => b.BlogTags)
            .ThenInclude(bt => bt.Tag)
            .Include(b => b.BlogCategories)
            .ThenInclude(bc => bc.Category)
            .ToListAsync();
    }

    public async Task<BlogEntity> CreateBlogAsync(BlogDTO blog)
    {
        var newBlog = MappingHelper.MappingBlogEntity(blog);
        await _Repository.AddAsync(newBlog);
        return newBlog;
    }

    public async Task<BlogEntity> UpdateBlogAsync(BlogDTO blog, int id)
    {
        var updatedBlog = _Repository.GetAll<BlogEntity>().FirstOrDefault(b => b.Id == id);
        if (updatedBlog == null)
        {
            return null;
        }
        updatedBlog.Title = blog.Title;
        updatedBlog.Content = blog.Content;
        updatedBlog.ImageUrl = blog.ImageUrl;
        await _Repository.UpdateAsync(updatedBlog);
        return updatedBlog;
    }

    public async Task DeleteBlogAsync(int blogId)
    {
        var blog = await _Repository.GetByIdAsync<BlogEntity>(blogId);
        if (blog != null)
        {
            await _Repository.DeleteAsync<BlogEntity>(blogId);
        }
    }

    public async Task AddTagToBlogAsync(int blogId, int tagId)
    {
        var blog = await _Repository.GetByIdAsync<BlogEntity>(blogId);
        var tag = await _Repository.GetByIdAsync<BlogTagEntity>(tagId);

        if (blog != null && tag != null)
        {
            var relBlogTag = new RelBlogTagEntity
            {
                BlogId = blogId,
                TagId = tagId
            };
            await _Repository.AddAsync(relBlogTag);
        }
    }

    public async Task AddCategoryToBlogAsync(int blogId, int categoryId)
    {
        var blog = await _Repository.GetByIdAsync<BlogEntity>(blogId);
        var category = await _Repository.GetByIdAsync<CategoryEntity>(categoryId);

        if (blog != null && category != null)
        {
            var relBlogCategory = new RelBlogCategoryEntity
            {
                BlogId = blogId,
                CategoryId = categoryId
            };
            await _Repository.AddAsync(relBlogCategory);
        }
    }
}
