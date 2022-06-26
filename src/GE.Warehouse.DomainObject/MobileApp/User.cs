using GE.Warehouse.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GE.Warehouse.DomainObject
{
    public class User: BaseEntity
    {
        public string Username { get; set; }

        public int Usermobi { get; set; }

        public UserStatus Status { get; set; }
        public object Id { get; set; }
    }

    public enum UserStatus
    {
        NotVerified = 0,
        Verified = 1,
        Disabled = 2,
    }
}
