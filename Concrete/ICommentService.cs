using App.Data.Entities;

namespace SimoshStore;

public interface ICommentService
{
    public Task<IEnumerable<ProductCommentEntity>> GetLatestComments();
    public Task<IServiceResult> ConfirmProductComment(int id);

    public Task<IServiceResult> GettingComments(int productId);
    public Task<IServiceResult> AddingCommentToProduct(ProductCommentEntity comment);
    public Task<int> ProductCommentCount();
    public Task<int> BlogCommentCount();

}
