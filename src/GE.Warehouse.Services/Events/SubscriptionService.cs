using System.Collections.Generic;
using GE.Warehouse.Core.Infrastructure;

namespace GE.Warehouse.Services.Events
{
    public class SubscriptionService : ISubscriptionService
    {
        public IList<IConsumer<T>> GetSubscriptions<T>()
        {
            return EngineContext.Current.ResolveAll<IConsumer<T>>();
        }
    }
}
