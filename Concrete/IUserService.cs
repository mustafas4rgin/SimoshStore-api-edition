using App.Data.Entities;

namespace SimoshStore;

public interface IUserService
{
    public Task<IServiceResult> ContactUsAsync(ContactDTO dto);
    public Task<IServiceResult> UpdateUserAsync(UpdateUserViewModel model);
    public Task<IServiceResult> DeleteUserAsync(int id);
    public Task<IServiceResult> GetAllUsersAsync();
    public Task<IServiceResult> GetUserAsync(int id);
    public Task<IServiceResult> UpdateUserAsync(UpdateProfileViewModel model, int id);
    public int GetUserId();
    public Task<IServiceResult> UpdateUserAddress(EditAddressViewModel model);
    public Task<UserEntity> GetUserByIdAsync(int id);
    public Task<UserEntity> GetUserByTokenAsync(string token);
    Task<int> UserCount();
}
