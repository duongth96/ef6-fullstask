using GE.Warehouse.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.Warehouse.Web.Models
{
    public class TransactionItemModel
    {
        public IOStatus Status { get; set; }

        public string QRCode { get; set; }

        public int Quantity { get; set; }
    }
}