using System.Threading.Tasks;
using App.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace SimoshStore;

public class BlogCategoryService : IBlogCategoryService
{
    IDataRepository _Repository;
    public BlogCategoryService(IDataRepository dataRepository)
    {
        _Repository = dataRepository;
    }
    public async Task<int> BlogCategoryCount()
    {
        return await _Repository.GetAll<BlogCategoryEntity>().CountAsync();
    }
    public async Task<BlogCategoryEntity> GetCategoryByIdAsync(int categoryId)
    {
        var category = await _Repository.GetByIdAsync<BlogCategoryEntity>(categoryId);
        if (category == null)
        {
            return new BlogCategoryEntity();
        }
        return category;
    }

    public async Task<IEnumerable<BlogCategoryEntity>> GetAllCategoriesAsync()
    {
        var categories = await _Repository.GetAll<BlogCategoryEntity>().ToListAsync();
        if(categories == null)
        {
            return new List<BlogCategoryEntity>();
        }
        return categories;
    }

    public async Task<BlogCategoryEntity> CreateCategoryAsync(BlogCategoryEntity category)
    {
        await _Repository.AddAsync(category);
        return category;
    }

    public async Task<BlogCategoryEntity> UpdateCategoryAsync(BlogCategoryEntity category)
    {
        await _Repository.UpdateAsync(category);
        return category;
    }

    public async Task DeleteCategoryAsync(int categoryId)
    {
        var category = await _Repository.GetByIdAsync<BlogCategoryEntity>(categoryId);
        if (category != null)
        {
            await _Repository.DeleteAsync<BlogCategoryEntity>(categoryId);
        }
    }
}
