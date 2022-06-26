using GE.Warehouse.Core.Data;
using GE.Warehouse.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GE.Warehouse.Services.MobiApp
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepo;

        public UserService(IRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public User Update(User user)
        {
            _userRepo.Update(user);
            return user;
        }

        public User ValidateUser(string username, int usermobi)
        {
            var user = (from u in _userRepo.Table
                       where u.Username == username && u.Status != UserStatus.Disabled
                       select u).SingleOrDefault();

            return user;
        }
    }
}
