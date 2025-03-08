using System.Security.Claims;
using App.Data.Entities;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;

namespace SimoshStore;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDataRepository _Repository;
    public UserService(IHttpContextAccessor httpContextAccessor, IDataRepository Repository)
    {
        _httpContextAccessor = httpContextAccessor;
        _Repository = Repository;
    }
    public async Task<IServiceResult> UpdateUserAsync(UpdateUserViewModel model)
    {
        var userId = GetUserId();
        var user = await _Repository.GetByIdAsync<UserEntity>(userId);
        if (user is null)
        {
            return new ServiceResult(false, "user not found");
        }
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Email = model.Email;
        user.Phone = model.Phone;
        await _Repository.UpdateAsync(user);
        return new ServiceResult(true, "user updated successfully");
    }
    public async Task<int> UserCount()
    {
        var users = _Repository.GetAll<UserEntity>();
        return await users.CountAsync();
    }
    public async Task<IServiceResult> ContactUsAsync(ContactDTO dto)
    {
        var form =MappingHelper.MappingContactForm(dto);
        await _Repository.AddAsync(form);
        return new ServiceResult(true, "message sent successfully");
    }
    public async Task<UserEntity> GetUserByTokenAsync(string token)
    {
        var user = await _Repository.GetAll<UserEntity>().FirstOrDefaultAsync(u => u.ResetToken == token);
        if (user == null)
        {
            return new UserEntity();
        }
        return user;
    }
    public async Task<IServiceResult> UpdateUserAddress(EditAddressViewModel model)
    {
        var userId = GetUserId();
        var user = await _Repository.GetByIdAsync<UserEntity>(userId);
        if (user is null)
        {
            return new ServiceResult(false, "user not found");
        }
        user.Address = $"{model.FirstName}, {model.LastName}, {model.Street}, {model.Phone} {model.PostCode}";

        await _Repository.UpdateAsync(user);
        return new ServiceResult(true, "address updated successfully");
    }
    public int GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User?.Claims
                        .FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return 0;
        }

        return int.TryParse(userId, out var id) ? id : 0;
    }
    public async Task<UserEntity> GetUserByIdAsync(int id)
    {
        var user = await _Repository.GetByIdAsync<UserEntity>(id);
        if(user == null)
        {
            return new UserEntity();
        }
        return user;
    }
    public async Task<IServiceResult> GetUserAsync(int id)
    {
        var user = await _Repository.GetByIdAsync<UserEntity>(id);
        if (user == null)
        {
            return new ServiceResult(false, "user not found");
        }
        return new ServiceResult(true, "user found");
    }
    public async Task<IServiceResult> DeleteUserAsync(int id)
    {
        var user = await _Repository.GetByIdAsync<UserEntity>(id);
        if (user == null)
        {
            return new ServiceResult(false, "user not found");
        }
        await _Repository.DeleteAsync<UserEntity>(id);
        return new ServiceResult(true, "user deleted successfully");
    }
    public async Task<IServiceResult> GetAllUsersAsync()
    {
        var users = _Repository.GetAll<UserEntity>();
        if (users == null)
        {
            return new ServiceResult(false, "no users found");
        }
        return new ServiceResult(true, "users found");
    }
    public async Task<IServiceResult> UpdateUserAsync(UpdateProfileViewModel model, int id)
    {
        var user = await _Repository.GetByIdAsync<UserEntity>(id);
        if (user is null)
        {
            return new ServiceResult(false, "user not found");
        }
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Email = model.Email;
        user.Phone = model.Phone;
        await _Repository.UpdateAsync(user);
        return new ServiceResult(true, "user updated successfully");
    }
}
