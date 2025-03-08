using App.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace SimoshStore;


public class ProfileService : IProfileService
{
    private readonly IEmailService _emailService;
    private readonly IDataRepository _Repository;
    public ProfileService(IEmailService emailService, IDataRepository repository)
    {
        _emailService = emailService;
        _Repository = repository;
    }
    public async Task<IEnumerable<BlogCommentEntity>> GetBlogCommentsByUserIdAsync()
    {
        var comments = await _Repository.GetAll<BlogCommentEntity>().ToListAsync();
        if (comments == null)
        {
            return new List<BlogCommentEntity>();
        }
        return comments;
    }
    public async Task<IServiceResult> UserConfirmation(int userId)
    {
        var user = await _Repository.GetByIdAsync<UserEntity>(userId);
        if (user == null)
        {
            return new ServiceResult(false, "user not found");
        }
        var updateToken = Guid.NewGuid().ToString();

        user.ResetToken = updateToken;
        user.ResetTokenExpires = DateTime.UtcNow.AddHours(1);

        await _Repository.UpdateAsync(user);

        var confirmationLink = $"https://localhost:5095/Profile/Confirmation?token={updateToken}";

        await _emailService.SendEmailAsync(user.Email, "Şifre Sıfırlama", $"Şifrenizi sıfırlamak için şu bağlantıyı tıklayın: {confirmationLink}");

        return new ServiceResult(true,"Account update link successfully sent.");
    }
    public async Task<UserEntity> GetUserByIdAsync(int userId)
    {
        var user = await _Repository.GetByIdAsync<UserEntity>(userId);
        if (user == null)
        {
            return new UserEntity();
        }
        return user;
    }
    public async Task<IEnumerable<ProductCommentEntity>> GetCommentByIdAsync(int userId)
    {
        var comments = await _Repository.GetAll<ProductCommentEntity>().Where(c => c.UserId == userId).ToListAsync();
        if (comments == null)
        {
            return new List<ProductCommentEntity>();
        }
        return comments;
    }
    public async Task<IEnumerable<ProductCommentEntity>> GetComments()
    {
        var comments = await _Repository.GetAll<ProductCommentEntity>().ToListAsync();
        if (comments == null)
        {
            return new List<ProductCommentEntity>();
        }
        return comments;
    }
}
