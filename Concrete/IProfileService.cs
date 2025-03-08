using App.Data.Entities;

namespace SimoshStore;

public interface IProfileService
{
    public Task<UserEntity> GetUserByIdAsync(int userId);
    public Task<IEnumerable<ProductCommentEntity>> GetCommentByIdAsync(int commentId);
    public Task<IEnumerable<ProductCommentEntity>> GetComments();
    public Task<IServiceResult> UserConfirmation(int userId);
}
