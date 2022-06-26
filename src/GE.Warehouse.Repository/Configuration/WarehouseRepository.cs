using GE.Warehouse.DomainObject;

namespace GE.Warehouse.Repository.EF.Configuration
{
    public class WarehouseRepository : MvcEntityTypeConfiguration<WHMobi>
    {
        public WarehouseRepository()
        {
            this.ToTable("WHMobi");
            this.HasKey(c => c.Id);
            this.Property(c => c.Name);
        }
    }
}
