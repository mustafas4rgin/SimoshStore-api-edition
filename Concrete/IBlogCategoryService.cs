using App.Data.Entities;

namespace SimoshStore;

public interface IBlogCategoryService
{
    Task<BlogCategoryEntity> GetCategoryByIdAsync(int categoryId);
    Task<IEnumerable<BlogCategoryEntity>> GetAllCategoriesAsync();
    Task<BlogCategoryEntity> CreateCategoryAsync(BlogCategoryEntity category);
    Task<BlogCategoryEntity> UpdateCategoryAsync(BlogCategoryEntity category);
    Task DeleteCategoryAsync(int categoryId);   
    Task<int> BlogCategoryCount();
    
}
