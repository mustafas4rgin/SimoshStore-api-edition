using App.Data.Entities;

namespace SimoshStore;

public interface ICategoryService
{
    public Task<IServiceResult> CreateCategoryAsync(CategoryDTO category);
    public Task<IServiceResult> UpdateCategoryAsync(CategoryDTO category, int id);
    public Task<IServiceResult> DeleteCategoryAsync(int id);
    public Task<IServiceResult> GetAllCategoriesAsync();
    public Task<IServiceResult> GetCategoryAsync(CategoryEntity category);
    public Task<List<CategoryEntity>> ListAllCategories();
    public Task<int> CategoryCount();
}
