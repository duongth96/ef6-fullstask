using GE.Warehouse.Core.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using System.Web.Http;

namespace GE.Warehouse.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //initialize engine context
            EngineContext.Initialize(false, config);

            // Web API configuration and services
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { action = "Index", id = RouteParameter.Optional }
            );
        }
    }
}
