using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.Warehouse.Web.Models
{
    public class TokenModel
    {
        public string AccessToken { get; set; }

        public DateTimeOffset ExpiresUtc { get; set; }
    }
}