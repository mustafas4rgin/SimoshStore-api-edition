using App.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace SimoshStore;

public class CategoryService : ICategoryService
{
    private readonly IDataRepository _Repository;
    public CategoryService(IDataRepository repository)
    {
        _Repository = repository;
    }
    public async Task<int> CategoryCount()
    {
        return await _Repository.GetAll<CategoryEntity>().CountAsync();
    }
    public async Task<List<CategoryEntity>> ListAllCategories()
    {
        var categories = await _Repository.GetAll<CategoryEntity>().ToListAsync();
        if(categories is not null)
        {
            return categories;
        }
        return new List<CategoryEntity>();
    }
    public async Task<IServiceResult> CreateCategoryAsync(CategoryDTO categoryDTO)
    {
        var newCategory = MappingHelper.MappingCategory(categoryDTO);
        var categories = _Repository.GetAll<CategoryEntity>();
        if (categories.Any(c => c.Name == newCategory.Name))
        {
            return new ServiceResult(false, "Category already exists");
        }
        await _Repository.AddAsync(newCategory);
        return new ServiceResult(true, "Category created successfully");
    }
    public async Task<IServiceResult> UpdateCategoryAsync(CategoryDTO CategoryDTO, int id)
    {
        var category = await _Repository.GetByIdAsync<CategoryEntity>(id);
        if (category == null)
        {
            return new ServiceResult(false, "Category not found");
        }
        category.Name = CategoryDTO.Name;
        category.Color = CategoryDTO.Color;
        category.IconCssClass = CategoryDTO.IconCssClass;
        await _Repository.UpdateAsync(category);
        return new ServiceResult(true, "Category updated successfully");
    }
    public async Task<IServiceResult> DeleteCategoryAsync(int id)
    {
        var category = await _Repository.GetByIdAsync<CategoryEntity>(id);
        var products = await _Repository.GetAll<ProductEntity>().Where(p => p.CategoryId == id).ToListAsync();
        
        if(products is not null)
        {
            return new ServiceResult(false, "Category has products");
        }
        if (category == null)
        {
            return new ServiceResult(false, "Category not found");
        }
        await _Repository.DeleteAsync<CategoryEntity>(id);
        return new ServiceResult(true, "Category deleted successfully");
    }
    public async Task<IServiceResult> GetAllCategoriesAsync()
    {
        var categories = _Repository.GetAll<CategoryEntity>();
        if(categories is null)
        {
            return new ServiceResult(false, "No categories found");
        }
        return new ServiceResult(true, "Categories found");
    }
    public async Task<IServiceResult> GetCategoryAsync(CategoryEntity category)
    {
        var categoryEntity = await _Repository.GetByIdAsync<CategoryEntity>(category.Id);
        if (categoryEntity == null)
        {
            return new ServiceResult(false, "Category not found");
        }
        return new ServiceResult(true, "Category found");
    }
}
