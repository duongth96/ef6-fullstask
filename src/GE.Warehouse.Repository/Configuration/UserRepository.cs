using GE.Warehouse.DomainObject;

namespace GE.Warehouse.Repository.EF.Configuration
{
    public class UserRepository: MvcEntityTypeConfiguration<User>
    {
        public UserRepository()
        {
            this.ToTable("_USERMOBI");
            this.HasKey(c => c.Id);
            this.Property(c => c.Usermobi);
            this.Property(c => c.Username);//.HasColumnName("Name") ;
            this.Property(c => c.Status);
        }
    }
}
