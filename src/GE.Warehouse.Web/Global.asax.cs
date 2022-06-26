using GE.Warehouse.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace GE.Warehouse.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var dependencyResolver = new MvcCoreDependencyResolver();
            DependencyResolver.SetResolver(dependencyResolver);
            ModelBinders.Binders.Add(typeof(BaseMvcModel), new MvcCoreModelBinder());
            ModelMetadataProviders.Current = new MvcCoreMetadataProvider();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
