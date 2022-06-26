using System.Web.Routing;

namespace GE.Warehouse.Web.Framework.Mvc.Routes
{
    public interface IRoutePublisher
    {
        void RegisterRoutes(RouteCollection routeCollection);
    }
}
