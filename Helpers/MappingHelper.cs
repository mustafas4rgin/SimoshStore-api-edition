using App.Data.Entities;

namespace SimoshStore;

public class MappingHelper
{
    public static OrderEntity MappingOrderEntity(OrderDTO dto)
    {
        return new OrderEntity
        {
            Address = dto.Address,
            OrderCode = dto.OrderCode,
            UserId = dto.UserId
        };
    }
    public static BlogCategoryEntity MappingBlogCategoryEntity(BlogCategoryEntityDTO dto)
    {
        return new BlogCategoryEntity
        {
            Name = dto.Name,
        };
    }
    public static BlogEntity MappingBlogEntity(BlogDTO dto)
    {
        return new BlogEntity
        {
            Title = dto.Title,
            Content = dto.Content,
            ImageUrl = dto.ImageUrl,
            UserId = dto.userId,
        };
    }
    public static ContactFormEntity MappingContactForm(ContactDTO dto)
    {
        return new ContactFormEntity
        {
            Name = dto.Name,
            Email = dto.Email,
            Message = dto.Message
        };
    }
    public static CategoryEntity MappingCategory(CategoryDTO dto)
    {
        return new CategoryEntity
        {
            Color = dto.Color,
            IconCssClass = dto.IconCssClass,
            Name = dto.Name
        };
    }
    public static ProductEntity MappingProduct(ProductDTO dto)
    {
        return new ProductEntity
        {
            CategoryId = dto.CategoryId,
            Description = dto.Description,
            DiscountId = dto.DiscountId,
            Price = dto.Price,
            Name = dto.Name,
            StockAmount = dto.StockAmount,
            Enabled = dto.Enabled,
        };
    }

    public static ProductImageEntity MappingProductImage(ProductImageDTO dto)
    {
        return new ProductImageEntity
        {
            Url = dto.Url,
            ProductId = dto.ProductId
        };
    }
}
