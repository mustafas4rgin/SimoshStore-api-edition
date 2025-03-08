using App.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace SimoshStore;

public class CommentService : ICommentService
{
    private readonly IDataRepository _Repository;
    public CommentService(IDataRepository repository)
    {
        _Repository = repository;
    }
    public async Task<IEnumerable<ProductCommentEntity>> GetLatestComments()
    {
        var latestComments = await _Repository.GetAll<ProductCommentEntity>().OrderByDescending(c => c.CreatedAt).Take(5).ToListAsync();
        foreach(var comment in latestComments)
        {
            var product = await _Repository.GetByIdAsync<ProductEntity>(comment.ProductId);
            if(product is not null)
            {
                comment.Product = product;
            }
            var user = await _Repository.GetByIdAsync<UserEntity>(comment.UserId);
            if(user is not null)
            {
                comment.User = user;
            }
            
        }
        return latestComments;
        
    }
    public async Task<IServiceResult> ConfirmProductComment(int id)
    {
        var comment = await _Repository.GetByIdAsync<ProductCommentEntity>(id);
        if(comment == null)
        {
            return new ServiceResult(false, "comment not found");
        }
        comment.IsConfirmed = true;
        await _Repository.UpdateAsync(comment);
        return new ServiceResult(true, "comment confirmed");
    }
    public async Task<int> ProductCommentCount()
    {
        return await _Repository.GetAll<ProductCommentEntity>().CountAsync();
    }
    public async Task<int> BlogCommentCount()
    {
        return await _Repository.GetAll<BlogCommentEntity>().CountAsync();
    }
    public async Task<IServiceResult> GettingComments(int productId)
    {
        var comments = _Repository.GetAll<ProductCommentEntity>();
        if(comments == null)
        {
            return new ServiceResult(false, "no comments found");
        }
        return new ServiceResult(true, "comments found");
    }
    public async Task<IServiceResult> AddingCommentToProduct(ProductCommentEntity comment)
    {
        var product = await _Repository.GetByIdAsync<ProductEntity>(comment.ProductId);
        if(product == null)
        {
            return new ServiceResult(false, "product not found");
        }
        await _Repository.AddAsync(comment);
        return new ServiceResult(true, "comment added successfully");
    }
}
