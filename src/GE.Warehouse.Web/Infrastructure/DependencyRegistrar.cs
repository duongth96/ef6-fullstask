using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using GE.Warehouse.Core;
using GE.Warehouse.Core.Caching;
using GE.Warehouse.Core.Configuration;
using GE.Warehouse.Core.Data;
using GE.Warehouse.Core.Fakes;
using GE.Warehouse.Core.Infrastructure;
using GE.Warehouse.Core.Infrastructure.DependencyManagement;
using GE.Warehouse.Core.IO;
using GE.Warehouse.Core.Plugins;
using GE.Warehouse.DomainObject.Common;
using GE.Warehouse.Repository;
using GE.Warehouse.Repository.EF;
using GE.Warehouse.Services.Configuration;
using GE.Warehouse.Services.Events;
using GE.Warehouse.Services.MobiApp;
using GE.Warehouse.Web.Framework.EmbeddedViews;
using GE.Warehouse.Web.Framework.Mvc.Routes;
using GE.Warehouse.Web.Framework.UI.Editor;

namespace GE.Warehouse.Web.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, MvcCoreConfig config)
        {
            // HTTP context and other related stuff
            builder.Register(c =>
                //register FakeHttpContext when HttpContext is not available
                HttpContext.Current != null ?
                (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
                (new FakeHttpContext("~/") as HttpContextBase))
                .As<HttpContextBase>()
                .InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();

            //web helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();

            //data layer
            builder.RegisterType<DataSettings>().InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();
            builder.RegisterType<SqlCeDataProvider>().As<IEfDataProvider>().InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();
            builder.RegisterType<SqlServerDataProvider>().As<IEfDataProvider>().InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();

            var dataSettingsManager = new DataSettingsManager();
            var dataProviderSettings = dataSettingsManager.LoadSettings();
            builder.Register(c => dataSettingsManager.LoadSettings()).As<DataSettings>();
            builder.Register(x => new EfDataProviderManager(x.Resolve<DataSettings>())).As<BaseDataProviderManager>().InstancePerDependency().InstancePerApiRequest().InstancePerLifetimeScope();

            builder.Register(x => x.Resolve<BaseDataProviderManager>().LoadDataProvider()).As<IDataProvider>().InstancePerDependency().InstancePerApiRequest().InstancePerLifetimeScope();

            if (dataProviderSettings != null && dataProviderSettings.IsValid())
            {
                var efDataProviderManager = new EfDataProviderManager(dataSettingsManager.LoadSettings());
                var dataProvider = efDataProviderManager.LoadDataProvider();
                dataProvider.InitDatabase();

                builder.Register<IDbContext>(c => new DbObjectContext(dataProviderSettings.DataConnectionString)).InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();
            }
            else
            {
                builder.Register<IDbContext>(c => new DbObjectContext(dataSettingsManager.LoadSettings().DataConnectionString)).InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();
            }


            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();

            builder.RegisterType<AppDomainTypeFinder>().As<ITypeFinder>().InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();

            //plugins
            builder.RegisterType<PluginFinder>().As<IPluginFinder>().InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();

            //cache manager
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>("webmvc_cache_static").SingleInstance();
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().Named<ICacheManager>("webmvc_cache_per_request").InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();


            //common
            builder.RegisterType<CommonSettings>().InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();

            //register file and email
            builder.RegisterType<FileWrapper>().As<IFile>().InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();

            //services
            builder.RegisterType<SettingService>().As<ISettingService>().InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();
            builder.RegisterType<EventPublisher>().As<IEventPublisher>().InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();
            builder.RegisterType<SubscriptionService>().As<ISubscriptionService>().InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();
            builder.RegisterType<ConfigurationService>().As<IConfigurationService>().InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();
            builder.RegisterType<WHMobiService>().As<IWHMobiService>().InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();
            builder.RegisterType<IOInventoryService>().As<IInventoryService>().InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();

            //register webhelper
            builder.RegisterType<EmbeddedViewResolver>().As<IEmbeddedViewResolver>().SingleInstance().InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();
            builder.RegisterType<RoutePublisher>().As<IRoutePublisher>().SingleInstance().InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();

            //HTML Editor services
            builder.RegisterType<NetAdvDirectoryService>().As<INetAdvDirectoryService>().InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();
            builder.RegisterType<NetAdvImageService>().As<INetAdvImageService>().InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();

            //Register event consumers
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
            {
                builder.RegisterType(consumer)
                    .As(consumer.FindInterfaces((type, criteria) =>
                    {
                        var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                        return isMatch;
                    }, typeof(IConsumer<>)))
                    .InstancePerHttpRequest().InstancePerApiRequest().InstancePerLifetimeScope();
            }
            builder.RegisterType<EventPublisher>().As<IEventPublisher>().SingleInstance();
            builder.RegisterType<SubscriptionService>().As<ISubscriptionService>().SingleInstance();

            //controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.RegisterModule<AutofacWebTypesModule>();
        }

        public int Order
        {
            get { return 0; }
        }
    }

    public class SettingsSource : IRegistrationSource
    {
        static readonly MethodInfo BuildMethod = typeof(SettingsSource).GetMethod(
            "BuildRegistration",
            BindingFlags.Static | BindingFlags.NonPublic);

        public IEnumerable<IComponentRegistration> RegistrationsFor(
                Service service,
                Func<Service, IEnumerable<IComponentRegistration>> registrations)
        {
            var ts = service as TypedService;
            if (ts != null && typeof(ISettings).IsAssignableFrom(ts.ServiceType))
            {
                var buildMethod = BuildMethod.MakeGenericMethod(ts.ServiceType);
                yield return (IComponentRegistration)buildMethod.Invoke(null, null);
            }
        }

        public bool IsAdapterForIndividualComponents { get { return false; } }
    }

}
