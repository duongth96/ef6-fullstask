using System.Data.Entity.ModelConfiguration;

namespace GE.Warehouse.Repository.EF
{
    public abstract class MvcEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : class
    {
        protected MvcEntityTypeConfiguration()
        {
            PostInitialize();
        }

        /// <summary>
        /// Developers can override this method in custom partial classes
        /// in order to add some custom initialization code to constructors
        /// </summary>
        protected virtual void PostInitialize()
        {
            
        }
    }
}