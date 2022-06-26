using System.Collections.Generic;

namespace GE.Warehouse.Services.Events
{
    public interface ISubscriptionService
    {
        IList<IConsumer<T>> GetSubscriptions<T>();
    }
}
