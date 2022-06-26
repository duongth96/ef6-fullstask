using GE.Warehouse.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.Warehouse.Web.Models
{
    public class InventoryTransactionModel
    {
        public int WarehouseId { get; set; }

        public TransactionItemModel[] Items { get; set; }

    }
}