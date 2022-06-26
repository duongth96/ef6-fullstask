using GE.Warehouse.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GE.Warehouse.Services.MobiApp
{
    public interface IUserService
    {
        User ValidateUser(string username, int usermobi);

        User Update(User user);
    }
}
