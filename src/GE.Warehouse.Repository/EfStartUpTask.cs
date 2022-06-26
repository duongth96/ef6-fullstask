using GE.Warehouse.Core;
using GE.Warehouse.Core.Data;
using GE.Warehouse.Core.Infrastructure;

namespace GE.Warehouse.Repository
{
    public class EfStartUpTask : IStartupTask
    {
        public void Execute()
        {
            var settings = EngineContext.Current.Resolve<DataSettings>();
            if (settings != null && settings.IsValid())
            {
                var provider = EngineContext.Current.Resolve<IEfDataProvider>();
                if (provider == null)
                    throw new NopException("No IDataProvider found");
                provider.SetDatabaseInitializer();
            }
        }

        public int Order
        {
            //ensure that this task is run first 
            get { return -1000; }
        }
    }
}
