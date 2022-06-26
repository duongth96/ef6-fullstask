using GE.Warehouse.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GE.Warehouse.DomainObject
{
    public class IOInventory: BaseEntity
    {
        public string QRCode { get; set; }

        public long Quantity { get; set; }

        public IOStatus Status { get; set; }

        public int WarehouseId { get; set; }

        public string WarehouseName { get; set; }

        public string Username { get; set; }

        public string TransactionId { get; set; }
    }

    public enum IOStatus
    {
        Export = 1,
        Import = 2,
        Inventory = 3,
    }
}
