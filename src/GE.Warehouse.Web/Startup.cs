using System.Web.Http;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup("GE.Warehouse", typeof(GE.Warehouse.Web.Startup))]

namespace GE.Warehouse.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            ConfigureAuth(app);
            app.UseWebApi(config);
        }
    }
}
