using App.Data.Entities;
using SimoshStore;
using System.Linq;

namespace SimoshStore
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _appDbContext;

        public AuthRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public UserEntity ValidateUser(string email, string password)
        {
            var user = _appDbContext.Users.SingleOrDefault(u => u.Email == email);
            if (user == null)
            {
                return null; 
            }

            if (!HashingHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null; 
            }

            return user; 
        }
        public List<UserEntity> GetUsers()
        {
            return _appDbContext.Users.ToList();
        }
        public List<RoleEntity> GetRoles()
        {
            return _appDbContext.Roles.ToList();
        }
    }
}
