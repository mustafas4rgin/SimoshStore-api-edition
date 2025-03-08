using App.Data.Entities;

namespace SimoshStore;

public interface IAuthRepository
{
    public List<UserEntity> GetUsers();
    UserEntity? ValidateUser(string email, string password);
    public List<RoleEntity> GetRoles();
}
