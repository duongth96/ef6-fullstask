using GE.Warehouse.DomainObject;

namespace GE.Warehouse.Repository.EF.Configuration
{
    public class IOInventoryRepository : MvcEntityTypeConfiguration<IOInventory>
    {
        public IOInventoryRepository()
        {
            this.ToTable("IOInventory");
            this.HasKey(c => c.Id);
            this.Property(c => c.QRCode);
            this.Property(c => c.Quantity);
            this.Property(c => c.Username);
            this.Property(c => c.WarehouseId);
            this.Property(c => c.WarehouseName);
            this.Property(c => c.TransactionId);
        }
    }
}
