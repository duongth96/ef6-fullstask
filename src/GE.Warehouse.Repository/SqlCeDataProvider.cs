using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using GE.Warehouse.Repository.EF;
using GE.Warehouse.Repository.Initializers;

namespace GE.Warehouse.Repository
{
    public class SqlCeDataProvider : BaseEfDataProvider
    {
        /// <summary>
        /// Get connection factory
        /// </summary>
        /// <returns>Connection factory</returns>
        public override IDbConnectionFactory GetConnectionFactory()
        {
            return new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");
        }

        /// <summary>
        /// Set database initializer
        /// </summary>
        public override void SetDatabaseInitializer()
        {
            //var initializer = new CreateDatabaseIfNotExists<NopObjectContext>();
            var initializer = new CreateCeDatabaseIfNotExists<DbObjectContext>();
            Database.SetInitializer(initializer);
        }

        /// <summary>
        /// A value indicating whether this data provider supports stored procedures
        /// </summary>
        public override bool StoredProceduredSupported
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a support database parameter object (used by stored procedures)
        /// </summary>
        /// <returns>Parameter</returns>
        public override DbParameter GetParameter()
        {
            return new SqlParameter();
        }
    }
}
